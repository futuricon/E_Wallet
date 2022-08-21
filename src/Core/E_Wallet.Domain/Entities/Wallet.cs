namespace E_Wallet.Domain.Entities;

public class Wallet : IAggregateRoot
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public decimal Balance { get; set; }
    public DateTime LastModifiedDate { get; set; } = DateTime.UtcNow;

    public string? UserId { get; set; }

    public virtual User? User { get; set; }
    public virtual IList<Transaction> Transactions { get; set; } = new List<Transaction>();
}
