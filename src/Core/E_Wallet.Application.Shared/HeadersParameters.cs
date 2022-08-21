using Microsoft.AspNetCore.Mvc;

namespace E_Wallet.Application.Shared
{
    public class HeadersParameters
    {
        [FromHeader(Name = "X-UserId")] 
        public string? UserId { get; set; }

        [FromHeader(Name = "X-Digest")]
        public string? Digest { get; set; }
    }
}
