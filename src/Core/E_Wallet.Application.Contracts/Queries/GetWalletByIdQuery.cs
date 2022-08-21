using MediatR;

namespace E_Wallet.Application.Contracts.Queries;

public sealed class GetWalletByIdQuery : IRequest<DataResult<WalletDto>>
{
    public string? Id { get; set; }
}
