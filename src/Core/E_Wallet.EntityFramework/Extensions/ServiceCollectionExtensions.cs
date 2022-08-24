using Microsoft.Extensions.DependencyInjection;
using E_Wallet.EntityFramework.Repositories;
using Microsoft.Extensions.Configuration;

namespace E_Wallet.EntityFramework;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DbConnection");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString).UseLazyLoadingProxies();
        });

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        return services;
    }
}