namespace E_Wallet.EntityFramework.Repositories;

internal class Repository<TEntity> : GenericRepository<TEntity, AppDbContext>, IRepository<TEntity>
    where TEntity : class, IAggregateRoot
{
    public Repository(
        AppDbContext context,
        IUnitOfWork unitOfWork) : base(context)
    {
        UnitOfWork = unitOfWork;
    }

    public IUnitOfWork UnitOfWork { get; }
}