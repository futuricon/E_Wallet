namespace E_Wallet.EntityFramework.Repositories;

internal class UnitOfWork : GenericUnitOfWork<AppDbContext>, IUnitOfWork
{
    public UnitOfWork(AppDbContext context) : base(context)
    {
    }
}
