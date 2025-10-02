using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RealEstate.Infrastructure.Common.Configuration;
using RealEstate.Infrastructure.Common.Data;
using RealEstate.Infrastructure.Modules.Owner.Repositories;
using RealEstate.Domain.Modules.Owner.Interfaces;

namespace RealEstate.Infrastructure.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Configuración de MongoDB
            services.Configure<MongoDbSettings>(
                configuration.GetSection("MongoDbSettings"));

            // Contexto MongoDB
            services.AddSingleton<MongoDbContext>();

            // Repositorios Owner
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            return services;
        }
    }
}
