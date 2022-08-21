using MediatR;

namespace E_Wallet.Application.Contracts.Commands;
public sealed class CreateWalletCommand : IRequest<DataResult>
{
    public decimal Balance { get; set; }
    public string? UserId { get; set; }
}
