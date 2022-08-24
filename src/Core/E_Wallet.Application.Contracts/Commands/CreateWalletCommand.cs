using MediatR;

namespace E_Wallet.Application.Contracts.Commands;
public sealed class CreateWalletCommand : BaseCommand, IRequest<DataResult>
{
    public decimal Balance { get; set; }
}
