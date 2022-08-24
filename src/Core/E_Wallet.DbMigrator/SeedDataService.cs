using E_Wallet.Domain.Entities;
using E_Wallet.EntityFramework.Data;

namespace E_Wallet.DbMigrator;

public class SeedDataService : BackgroundService
{
    private readonly ILogger<SeedDataService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SeedDataService(
        ILogger<SeedDataService> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
            var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await AddDefaultUsersAsync(configuration, appDbContext);
            await AddDefaultWalletsAsync(configuration, appDbContext);
            await AddDefaultTransactionsAsync(configuration, appDbContext);
                    
            _logger.LogInformation("Data successfully seeded");
        }
        catch (Exception ex)
        {
            _logger.LogError("Thrown exception in SeedDataService.ExecuteAsync(): {Exception}", ex);
        }
    }

    //Sedd some default Users
    private async Task AddDefaultUsersAsync(IConfiguration configuration, AppDbContext appDbContext) 
    {
        var testUsers = configuration.GetSection("TestUsers").Get<UserDto[]>();
        var apiKey = configuration.GetSection("UserSecretKey").Value;

        foreach (var testUser in testUsers)
        {
            var user = appDbContext.Set<User>().FirstOrDefault(i => i.Id == testUser.Id);

            if (user != null)
            continue;

            user = new User
            {
                Id = testUser.Id!,
                WalletId = testUser.WalletId!,
                APIKey = apiKey,
                UserName = testUser.UserName!,
                IsIdentified = testUser.IsIdentified
            };

            ////used to update 
            //if (user == null)
            //    continue;

            //user.WalletId = testUser.WalletId!;
            //user.APIKey = apiKey;

            appDbContext.Update(user);
            await appDbContext.SaveChangesAsync();
            _logger.LogInformation("Created the test user {UserName}", testUser.UserName);
        }
    }

    //Sedd some default Wallets
    private async Task AddDefaultWalletsAsync(IConfiguration configuration, AppDbContext appDbContext) 
    {
        var testWallets = configuration.GetSection("TestWallets").Get<WalletDto[]>();

        foreach (var testWallet in testWallets)
        {
            var wallet = appDbContext.Set<Wallet>().FirstOrDefault(i => i.Id == testWallet.Id);

            if (wallet != null)
                continue;

            wallet = new Wallet
            {
                Id = testWallet.Id!,
                UserId = testWallet.UserId,
                Balance = testWallet.Balance!
            };

            appDbContext.Add(wallet);
            await appDbContext.SaveChangesAsync();
            _logger.LogInformation("Wallet created");
        }
    }

    //Sedd some default Transactions
    private async Task AddDefaultTransactionsAsync(IConfiguration configuration, AppDbContext appDbContext)
    {
        var testTransactions = configuration.GetSection("TestTransactions").Get<TransactionDto[]>();
        
        foreach (var testTransaction in testTransactions)
        {
            var transaction = appDbContext.Set<Transaction>().FirstOrDefault(i => i.Id == testTransaction.Id);

            if (transaction != null)
                continue;

            transaction = new Transaction
            {
                Id = testTransaction.Id!,
                UserId = testTransaction.UserId,
                WalletId = testTransaction.WalletId,
                TransactionTypeId = testTransaction.TransactionTypeId,
                Amount = testTransaction.Amount,
                TransactionDate = DateTime.ParseExact(testTransaction.TransactionDate!, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture)
            };

            appDbContext.Add(transaction);
            await appDbContext.SaveChangesAsync();
            _logger.LogInformation("Wallet created");
        }
    }
}

internal record UserDto
{
    public string? Id { get; init; }
    public string? WalletId { get; init; }
    public string? UserName { get; init; }
    public bool IsIdentified { get; init; }
}

internal record WalletDto
{
    public string? Id { get; init; }
    public string? UserId { get; init; }
    public decimal Balance { get; init; }
}

internal record TransactionDto
{
    public string? Id { get; init; }
    public string? WalletId { get; init; }
    public string? UserId { get; init; }
    public int TransactionTypeId { get; init; }
    public decimal Amount { get; init; }
    public string? TransactionDate { get; init; }
}