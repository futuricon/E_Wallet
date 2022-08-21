namespace E_Wallet.Application.Contracts.Models;

public class TransactionDto
{
    public string? Id { get; set; }

    [Required]
    public string? UserId { get; set; }

    [Required]
    public string? WalletId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    [Required]
    public int TransactionTypeId { get; set; }

    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}
