using Business.Models;
using Business.RabbitMQ;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Business.SignalR;

namespace Business
{
    public static class BusinessServiceRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IRabbitMQConnection, RabbitMQConnection>();
            services.Configure<ConnectionOptions>(configuration.GetSection(key: nameof(ConnectionOptions)));
            services.AddSignalR();
            return services;
        }
    }
}
