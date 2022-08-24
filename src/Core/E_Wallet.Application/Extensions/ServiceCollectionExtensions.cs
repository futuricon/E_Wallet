using Microsoft.Extensions.DependencyInjection;
using E_Wallet.Application.Mappers;

namespace E_Wallet.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(o =>
            {
                o.AddProfile<TransactionProfile>();
                o.AddProfile<WalletProfile>();
                //o.AddProfile<UserProfile>();
            });

            services.AddMediatR(typeof(ServiceCollectionExtensions).Assembly);
            return services;
        }
    }
}