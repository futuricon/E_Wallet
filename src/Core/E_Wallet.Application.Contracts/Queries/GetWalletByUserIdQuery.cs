using MediatR;

namespace E_Wallet.Application.Contracts.Queries;

public sealed class GetWalletByUserIdQuery : IRequest<DataResult<WalletDto>>
{
    public string? UserId { get; set; }
}
