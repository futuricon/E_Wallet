using MediatR;

namespace E_Wallet.Application.Contracts.Commands;
public sealed class CreateUserCommand : IRequest<DataResult>
{
    public string? WalletId { get; set; }
    public string? UserName { get; set; }
    public bool IsIdentified { get; set; }
}
