using Microsoft.EntityFrameworkCore;
using E_Wallet.DbMigrator;
using E_Wallet.EntityFramework.Data;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        var dbConnection = ctx.Configuration.GetConnectionString("DbConnection");
       
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(dbConnection).UseLazyLoadingProxies();
        });

        services.AddHostedService<SeedDataService>();
    })
    .Build();

await host.RunAsync();
