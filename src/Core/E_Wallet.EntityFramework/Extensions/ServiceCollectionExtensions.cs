using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using E_Wallet.EntityFramework.Repositories;
using E_Wallet.EntityFramework.Helpers;

namespace E_Wallet.EntityFramework;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(
        this IServiceCollection services,
        IConfiguration configuration,
        string connectionStringName = "Local")
        //,string tenantsConfigSection = "TenantsConfig")
    {
        var connectionString = configuration.GetConnectionString(connectionStringName);
        //var tenantsSettings = configuration.GetSection(tenantsConfigSection).Get<TenantsSettings>();

        //if (tenantsSettings is { DatabaseProvider: "mysql" })
        //{
        //    services.AddScoped<IDatabaseProviderService, MySqlProviderService>();
        //    services.AddSingleton(tenantsSettings);
        //}

        services.AddDbContext<AppDbContext>(o => DbContextHelpers.ConfigureMySql(connectionString, o));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        return services;
    }
}