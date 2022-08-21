namespace E_Wallet.Application.Contracts.Models;

public class WalletDto
{
    public string? Id { get; set; }

    public string? UserId { get; set; }

    public decimal Balance { get; set; }

    public DateTime LastModifiedDate { get; set; } = DateTime.Now;
}
