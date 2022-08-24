namespace E_Wallet.Application.Contracts.Commands;

public class BaseCommand
{
    public string? UserId { get; private set; }

    public void SetData(string? userId)
    {
        UserId = userId;
    }
}
