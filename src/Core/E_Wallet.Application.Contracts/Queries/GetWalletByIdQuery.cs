using MediatR;

namespace E_Wallet.Application.Contracts.Queries;

public sealed class GetWalletByIdQuery : BaseQuery, IRequest<DataResult<WalletDto>>
{
    public string? Id { get; set; }
}
