namespace E_Wallet.Domain.Repositories;

public interface IRepository<TEntity> : IGenericRepository<TEntity>
    where TEntity : class, IAggregateRoot
{
    /// <summary>
    /// Unit of work
    /// </summary>
    IUnitOfWork UnitOfWork { get; }
}