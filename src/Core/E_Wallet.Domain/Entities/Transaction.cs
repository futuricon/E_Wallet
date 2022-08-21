namespace E_Wallet.Domain.Entities;

public class Transaction : IAggregateRoot
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public decimal Amount { get; set; }
    public int TransactionTypeId { get; set; }
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    public string? UserId { get; set; }
    public string? WalletId { get; set; }

    public virtual User? User { get; set; }
    public virtual Wallet? Wallet { get; set; }
}
