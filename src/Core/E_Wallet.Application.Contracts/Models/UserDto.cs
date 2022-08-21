namespace E_Wallet.Application.Contracts.Models;

public class UserDto
{
    public UserDto()
    {
        UserName = string.Empty;
    }

    public string? Id { get; set; }

    [Required]
    public string UserName { get; set; }

    public string? AppId { get; set; }

    public string? APIKey { get; set; }

    public string? WalletId { get; set; }

    public bool IsIdentified { get; set; }
}