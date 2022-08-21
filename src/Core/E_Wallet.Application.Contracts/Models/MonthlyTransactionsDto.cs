namespace E_Wallet.Application.Contracts.Models;

public class MonthlyTransactionsDto
{
    [Required]
    public int Quantity { get; set; }

    [Required]
    public decimal Amount { get; set; }
}
