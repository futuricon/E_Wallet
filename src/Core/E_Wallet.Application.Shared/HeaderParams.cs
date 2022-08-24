using Microsoft.AspNetCore.Mvc;

namespace E_Wallet.Application.Shared
{
    public class HeaderParams
    {
        [FromHeader(Name ="X-UserId")] 
        public string? XUserId { get; set; }

        [FromHeader(Name = "X-Digest")]
        public string? XDigest { get; set; }
    }
}