namespace E_Wallet.WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class WalletController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public WalletController(
        IMapper mapper,
        IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost("balance")]
    //[ServiceFilter(typeof(HMACAuthenticationAttribute))]
    //[ProducesResponseType(typeof(DataResult<decimal>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetBalanceById([FromBody] string id)
    {
        var result = await _mediator.Send(new GetWalletBalanceByIdQuery
        {
            Id = id
        });

        if (result.Success)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("walletExists")]
    //[ServiceFilter(typeof(HMACAuthenticationAttribute))]
    //[ProducesResponseType(typeof(DataResult<WalletDto>), StatusCodes.Status200OK)]
    //[ProducesResponseType(typeof(DataResult), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetWalletByUserId([FromBody] string id)
    {
        var result = await _mediator.Send(new GetWalletByUserIdQuery
        {
            UserId = id
        });

        if (result.Success)
        {

            return Ok(result);
        }

        return BadRequest(result);
    }
}