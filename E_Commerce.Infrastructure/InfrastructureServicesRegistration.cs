using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Data;
using E_Commerce.Infrastructure.Repositories;
using E_Commerce.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace E_Commerce.Infrastructure
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices( this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddKeyedScoped<IDataSeeder, CatalogDataSeeder>("Catalog");
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
