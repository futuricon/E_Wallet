namespace E_Wallet.Domain;

/// <summary>
/// Aggregate root
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    /// Primary key
    /// </summary>
    public string Id { get; set; }
}