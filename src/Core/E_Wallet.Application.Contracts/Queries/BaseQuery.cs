namespace E_Wallet.Application.Contracts.Queries;

public class BaseQuery
{
    public string? UserId { get; private set; }

    public void SetData(string? userId)
    {
        UserId = userId;
    }
}
