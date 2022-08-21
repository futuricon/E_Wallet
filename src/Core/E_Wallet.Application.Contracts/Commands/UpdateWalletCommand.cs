using MediatR;

namespace E_Wallet.Application.Contracts.Commands;

public sealed class UpdateWalletCommand : IRequest<DataResult>
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public decimal Balance { get; set; }
}
