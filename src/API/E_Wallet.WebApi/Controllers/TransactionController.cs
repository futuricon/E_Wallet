namespace E_Wallet.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class TransactionController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public TransactionController(
        IMapper mapper,
        IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost("monthlyTansactions")]
    //[TypeFilter(typeof(HMACAuthenticationAttribute))]
    //[HMACAuthentication]
    //[ProducesResponseType(typeof(PagedDataResult<LoadDto>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetMonthlyTansactions([FromHeader] HeadersParameters parameters, [FromBody] GetTransactionsQuery request)
    {
        var result = await _mediator.Send(request);

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("replenish")]
    //[ServiceFilter(typeof(HMACAuthenticationAttribute))]
    //[ProducesResponseType(typeof(DataResult), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] TransactionDto request)
    {
        var resultTransaction = await _mediator.Send(_mapper.Map<CreateTransactionCommand>(request));

        if (!resultTransaction.Success)
            return BadRequest(resultTransaction);

        var resultWallet = await _mediator.Send(new UpdateWalletCommand
        {
            Id = request.WalletId,
            UserId = request.UserId,
            Balance = resultTransaction.Value
        });

        if (!resultWallet.Success)
            return BadRequest(resultWallet);

        return Ok(resultTransaction);
    }
}