using Azure.Storage.Blobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexer.Weather.Application.Services;
using Nexer.Weather.Infrastructure.Services;

namespace Nexer.Weather.Infrastructure
{
    public static class Configuration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IDataService, DataService>();

            services.AddTransient<BlobContainerClient>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                return new BlobContainerClient(configuration["BlobConnectionString"], configuration["BlobContainerName"]);
            });

            return services;
        }
    }
}
