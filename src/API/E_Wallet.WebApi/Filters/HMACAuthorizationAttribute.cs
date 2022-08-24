using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.WebApiCompatShim;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;

namespace E_Wallet.WebApi.Filters;

public class HMACAuthorizationAttribute : ActionFilterAttribute //, Attribute, IAsyncActionFilter
{
    private readonly string _secretKey;

    public HMACAuthorizationAttribute(IConfiguration configuration)
    {
        //!!!"UserSecretKey" should be moved to the secrets.json file or to the DB !!!
        _secretKey = configuration.GetSection("UserSecretKey").Value;
    }
    public override void OnActionExecuting(ActionExecutingContext context) 
    {
        if (context != null)
        {
            var content = JsonSerializer.Serialize(context.ActionArguments["request"]);
            var hreqmf = new HttpRequestMessageFeature(context.HttpContext);
            var httprequestMessage = hreqmf.HttpRequestMessage;

            if (httprequestMessage == null)
                throw new ArgumentNullException(nameof(httprequestMessage));

            var userId = httprequestMessage.Headers.GetValues("X-UserId").FirstOrDefault();
            var digest = httprequestMessage.Headers.GetValues("X-Digest").FirstOrDefault();

            if (userId == null || digest == null)
                context.Result = CreateUnauthorized();

            var isValid = IsValidated(httprequestMessage, userId, digest, content);
            if (!isValid.Result)
                context.Result = CreateUnauthorized();
        }
        static IActionResult CreateUnauthorized() => new UnauthorizedObjectResult(new ErrorMessage("Unauthorized", 401));
    }

    #region Private Methds

    private async Task<bool> IsValidated(HttpRequestMessage request, string userId, string requestSignature, string content)
    {
        bool isValidated = false;
        string requestContentBase64String = "";

        //get request uri
        string requestUri = HttpUtility.UrlEncode(request.RequestUri.AbsoluteUri.ToLower());

        //get the request method(get / post etc..)
        string requestHttpMethod = request.Method.Method.ToLower();

        //compute signature to match with request signature
        //hash  request content
        byte[] hash = await ComputeHash(content);
        if (hash != null)
        {
            //get Base64 string from the content
            requestContentBase64String = Convert.ToBase64String(hash);
        }

        //populate the string to be converted to signature
        //should contain userId, requestHttpMethod, requestUri and request content in the given order
        string signatureData = String.Format("{0}{1}{2}{3}", userId, requestHttpMethod, requestUri, requestContentBase64String);

        //get the signature data in bytes
        var signatureDataBytes = Encoding.UTF8.GetBytes(signatureData);

        //get the secret key in bytes
        var secretKeyBytes = Convert.FromBase64String(_secretKey);

        //compute hmac hash using the secret key
        using (HMACSHA1 hmac = new HMACSHA1(secretKeyBytes))
        {
            byte[] computedSignatureHash = hmac.ComputeHash(signatureDataBytes);
            string serverSignature = Convert.ToBase64String(computedSignatureHash);
            
            //check computed signature hash and request signature are equal
            if (serverSignature.Equals(requestSignature, StringComparison.OrdinalIgnoreCase))
            {
                //hashes are matching
                isValidated = true;
            }
        }

        //hashes do not match
        return isValidated;
    }

    private static async Task<byte[]> ComputeHash(string requestContent)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] hash = null;
            var content = Encoding.UTF8.GetBytes(requestContent);
            if (content.Length != 0)
            {
                hash = md5.ComputeHash(content);
            }
            return hash;
        }
    }

    #endregion Private Methods
}

internal class ErrorMessage
{
    private string v1;
    private int v2;

    public ErrorMessage(string v1, int v2)
    {
        this.v1 = v1;
        this.v2 = v2;
    }
}
