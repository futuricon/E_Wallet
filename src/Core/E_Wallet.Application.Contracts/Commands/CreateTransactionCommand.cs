using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace E_Wallet.Application.Contracts.Commands;
public sealed class CreateTransactionCommand : BaseCommand, IRequest<DataResult<decimal>>
{
    public string? WalletId { get; set; }
    public decimal Amount { get; set; }
    public int TransactionTypeId { get; set; }
}
