namespace E_Wallet.EntityFramework.Data;

public class AppDbContext : DbContext
{
    public AppDbContext()
    {

    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Transaction> Transactions { get; set; }
    public virtual DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<User>()
            .HasOne(s => s.Wallet)
            .WithOne(ad => ad.User)
            .HasForeignKey<Wallet>(ad => ad.UserId);

        builder.Entity<User>()
            .HasMany(s => s.Transactions)
            .WithOne(ad => ad.User);

        builder.Entity<Wallet>()
            .HasMany(s => s.Transactions)
            .WithOne(ad => ad.Wallet);
    }

}
