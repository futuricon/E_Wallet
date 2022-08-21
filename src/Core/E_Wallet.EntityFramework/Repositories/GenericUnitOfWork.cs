namespace E_Wallet.EntityFramework.Repositories;

internal class GenericUnitOfWork<TContext> : IGenericUnitOfWork where TContext : DbContext
{
    private readonly TContext _context;

    public GenericUnitOfWork(TContext context)
    {
        _context = context;
    }

    public Task<int> CommitAsync()
    {
        return _context.SaveChangesAsync();
    }
}
