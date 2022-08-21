namespace E_Wallet.Domain.Entities;

public class User : IAggregateRoot
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? AppId { get; set; }
    public string? APIKey { get; set; }
    public string UserName { get; set; } = default!;
    public bool IsIdentified { get; set; }

    public string? WalletId { get; set; }

    public virtual Wallet? Wallet { get; set; }
    public virtual IList<Transaction> Transactions { get; set; } = new List<Transaction>();
}
