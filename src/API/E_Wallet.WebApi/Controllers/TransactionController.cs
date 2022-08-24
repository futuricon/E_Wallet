using E_Wallet.WebApi.Filters;

namespace E_Wallet.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
//[HMACAuthorization]
[ServiceFilter(typeof(HMACAuthorizationAttribute))]
public class TransactionController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransactionController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("monthlyTansactions")]
    //[ServiceFilter(typeof(HMACAuthorizationAttribute))]
    //[ProducesResponseType(typeof(DataResult<LoadDto>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMonthlyTansactions([FromHeader] HeaderParams headerParams, [FromBody] GetTransactionsQuery request)
    {
        request.SetData(headerParams.XUserId);
        var result = await _mediator.Send(request);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("replenish")]
    //[ProducesResponseType(typeof(DataResult), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromHeader] HeaderParams headerParams, [FromBody] CreateTransactionCommand request)
    {
        request.SetData(headerParams.XUserId);
        var resultTransaction = await _mediator.Send(request);

        if (!resultTransaction.Success)
            return BadRequest(resultTransaction);

        var resultWallet = await _mediator.Send(new UpdateWalletCommand
        {
            Id = request.WalletId,
            Balance = resultTransaction.Value
        });

        if (!resultWallet.Success)
            return BadRequest(resultWallet);

        return Ok(resultTransaction);
    }
}