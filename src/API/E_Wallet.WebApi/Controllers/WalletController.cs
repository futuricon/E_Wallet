using E_Wallet.WebApi.Filters;

namespace E_Wallet.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
//[HMACAuthorization]
[ServiceFilter(typeof(HMACAuthorizationAttribute))]
public class WalletController : ControllerBase
{
    private readonly IMediator _mediator;

    public WalletController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("balance")]
    //[ProducesResponseType(typeof(DataResult<decimal>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBalanceById([FromHeader] HeaderParams headerParams, [FromBody] GetWalletBalanceByIdQuery request)
    {
        request.SetData(headerParams.XUserId);
        var result = await _mediator.Send(request);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("walletExists")]
    //[ProducesResponseType(typeof(DataResult<WalletDto>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetWalletByUserId([FromHeader] HeaderParams headerParams, [FromBody] GetWalletByUserIdQuery request)
    {
        request.SetData(headerParams.XUserId);
        var result = await _mediator.Send(request);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }
}