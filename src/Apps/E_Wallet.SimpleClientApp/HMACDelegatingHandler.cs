using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace E_Wallet.SimpleClientApp;

public class HMACDelegatingHandler : DelegatingHandler
{
    // First obtained the UserId and API Key from the server
    // The APIKey MUST be stored securely in db or in the App.Config
    private readonly string UserId;
    private readonly string APIKey = "WLUEWeL3so2hdHhHM5ZYnvzsOUBzSGH4+T3EgrQ91KI=";

    public HMACDelegatingHandler(string userId) 
    {
        UserId = userId;
    }

    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = null;
        string requestContentBase64String = string.Empty;

        //Get the Request URI
        string requestUri = HttpUtility.UrlEncode(request.RequestUri!.AbsoluteUri.ToLower());

        //Get the Request HTTP Method type
        string requestHttpMethod = request.Method.Method.ToLower();

        //Checking if the request contains body, usually will be null wiht HTTP GET and DELETE
        if (request.Content != null)
        {
            // Hashing the request body, so any change in request body will result a different hash
            // we will achieve message integrity
            byte[] content = await request.Content.ReadAsByteArrayAsync();
            MD5 md5 = MD5.Create();
            byte[] requestContentHash = md5.ComputeHash(content);
            requestContentBase64String = Convert.ToBase64String(requestContentHash);
        }

        //Creating the raw signature string by combinging
        //UserId, request Http Method, request Uri and request Content Base64 String
        string signatureRawData = String.Format("{0}{1}{2}{3}", UserId.ToLower(), requestHttpMethod, requestUri, requestContentBase64String);
        
        //Converting the APIKey into byte array
        var secretKeyByteArray = Convert.FromBase64String(APIKey);

        //Converting the signatureRawData into byte array
        var signature = Encoding.UTF8.GetBytes(signatureRawData);

        //Generate the hmac signature and set it in the X-Digest header
        using (HMACSHA1 hmac = new HMACSHA1(secretKeyByteArray))
        {
            byte[] signatureBytes = hmac.ComputeHash(signature);
            string requestSignatureBase64String = Convert.ToBase64String(signatureBytes);

            //Setting the value in the X-Digest header
            request.Headers.Add("X-Digest", requestSignatureBase64String);
        }

        //Setting the value in the X-UserId header
        request.Headers.Add("X-UserId", UserId);

        response = await base.SendAsync(request, cancellationToken);
        return response;
    }
}
