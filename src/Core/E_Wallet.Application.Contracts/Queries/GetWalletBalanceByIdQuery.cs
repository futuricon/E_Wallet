using MediatR;

namespace E_Wallet.Application.Contracts.Queries;

public sealed class GetWalletBalanceByIdQuery : IRequest<DataResult<decimal>>
{
    public string? Id { get; set; }
}
