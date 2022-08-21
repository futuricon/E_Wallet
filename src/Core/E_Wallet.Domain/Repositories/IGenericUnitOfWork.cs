namespace E_Wallet.Domain.Repositories;

/// <summary>
/// Unit of work pattern
/// </summary>
public interface IGenericUnitOfWork
{
    /// <summary>
    /// Save changes to database
    /// </summary>
    /// <returns>Number of rows modified after save changes.</returns>
    Task<int> CommitAsync();
}
