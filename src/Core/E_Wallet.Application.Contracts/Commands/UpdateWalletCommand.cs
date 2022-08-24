using MediatR;

namespace E_Wallet.Application.Contracts.Commands;

public sealed class UpdateWalletCommand : BaseCommand, IRequest<DataResult>
{
    public string? Id { get; set; }
    public decimal Balance { get; set; }
}
