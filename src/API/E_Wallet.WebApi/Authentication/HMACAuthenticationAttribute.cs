using System.Net.Http.Headers;
using System.Runtime.Caching;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Http.Filters;
using UnauthorizedResult = System.Web.Http.Results.UnauthorizedResult;

namespace E_Wallet.WebApi;

//public class HMACAuthenticationAttribute : Attribute, IAuthenticationFilter
//{
//    private static string? _appId;
//    private static string? _apiKey;
//    private readonly ulong _requestMaxAgeInSeconds = 300; //Means 5 min
//    private readonly IMediator _mediator;

//    public HMACAuthenticationAttribute(IMediator mediator)
//    {
//        _mediator = mediator;
//    }

//    public async void GetAllowedUser(string? userId)
//    {
//        var result = await _mediator.Send(new GetUserByIdQuery
//        { 
//            Id = userId 
//        });

//        if (result.Success)
//        {
//            _appId = result.Value.AppId;
//            _apiKey = result.Value.APIKey;
//        }
//    }

//    public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
//    {
//        var req = context.Request;

//        if (req.Headers.GetValues("X-UserId") != null && req.Headers.GetValues("X-Digest") != null)
//        {
//            var rawAuthzHeader = req.Headers.GetValues("X-Digest");

//            GetAllowedUser(req.Headers.GetValues("X-UserId").FirstOrDefault());
            
//            var autherizationHeaderArray = GetAutherizationHeaderValues(rawAuthzHeader.FirstOrDefault());

//            if (autherizationHeaderArray != null && _appId != null && _apiKey != null)
//            {
//                var APPId = autherizationHeaderArray[0];
//                var incomingBase64Signature = autherizationHeaderArray[1];
//                var nonce = autherizationHeaderArray[2];
//                var requestTimeStamp = autherizationHeaderArray[3];

//                var isValid = IsValidRequest(req, APPId, incomingBase64Signature, nonce, requestTimeStamp);

//                if (isValid.Result)
//                {
//                    var currentPrincipal = new GenericPrincipal(new GenericIdentity(APPId), null);
//                    context.Principal = currentPrincipal;
//                }
//                else
//                {
//                    context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
//                }
//            }
//            else
//            {
//                context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
//            }
//        }
//        else
//        {
//            context.ErrorResult = new UnauthorizedResult(new AuthenticationHeaderValue[0], context.Request);
//        }

//        return Task.FromResult(0);
//    }

//    public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
//    {
//        context.Result = new ResultWithChallenge(context.Result);
//        return Task.FromResult(0);
//    }

//    public bool AllowMultiple
//    {
//        get { return false; }
//    }

//    private string[] GetAutherizationHeaderValues(string? rawAuthzHeader)
//    {

//        var credArray = rawAuthzHeader.Split(':');

//        if (credArray.Length == 4)
//        {
//            return credArray;
//        }
//        else
//        {
//            return null;
//        }
//    }

//    private async Task<bool> IsValidRequest(HttpRequestMessage req, string APPId, string incomingBase64Signature, string nonce, string requestTimeStamp)
//    {
//        string requestContentBase64String = "";
//        string requestUri = HttpUtility.UrlEncode(req.RequestUri.AbsoluteUri.ToLower());
//        string requestHttpMethod = req.Method.Method;

//        if (_appId != APPId)
//        {
//            return false;
//        }

//        var sharedKey = _apiKey;

//        if (IsReplayRequest(nonce, requestTimeStamp))
//        {
//            return false;
//        }

//        byte[] hash = await ComputeHash(req.Content);

//        if (hash != null)
//        {
//            requestContentBase64String = Convert.ToBase64String(hash);
//        }

//        string data = String.Format("{0}{1}{2}{3}{4}{5}", APPId, requestHttpMethod, requestUri, requestTimeStamp, nonce, requestContentBase64String);

//        var secretKeyBytes = Convert.FromBase64String(sharedKey);

//        byte[] signature = Encoding.UTF8.GetBytes(data);

//        using (HMACSHA1 hmac = new HMACSHA1())
//        {
//            byte[] signatureBytes = hmac.ComputeHash(signature);

//            return (incomingBase64Signature.Equals(Convert.ToBase64String(signatureBytes), StringComparison.Ordinal));
//        }

//    }

//    private bool IsReplayRequest(string nonce, string requestTimeStamp)
//    {
//        if (MemoryCache.Default.Contains(nonce))
//        {
//            return true;
//        }

//        DateTime epochStart = new DateTime(1970, 01, 01, 0, 0, 0, 0, DateTimeKind.Utc);
//        TimeSpan currentTs = DateTime.UtcNow - epochStart;

//        var serverTotalSeconds = Convert.ToUInt64(currentTs.TotalSeconds);
//        var requestTotalSeconds = Convert.ToUInt64(requestTimeStamp);

//        if ((serverTotalSeconds - requestTotalSeconds) > _requestMaxAgeInSeconds)
//        {
//            return true;
//        }

//        MemoryCache.Default.Add(nonce, requestTimeStamp, DateTimeOffset.UtcNow.AddSeconds(_requestMaxAgeInSeconds));

//        return false;
//    }

//    private static async Task<byte[]> ComputeHash(HttpContent httpContent)
//    {
//        using (MD5 md5 = MD5.Create())
//        {
//            byte[] hash = null;
//            var content = await httpContent.ReadAsByteArrayAsync();
//            if (content.Length != 0)
//            {
//                hash = md5.ComputeHash(content);
//            }
//            return hash;
//        }
//    }
//}
