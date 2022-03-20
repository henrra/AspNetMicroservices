using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Models;
using Ordering.Application.Contracts.Persistence;
using Ordering.Infrastructure.Mail;
using Ordering.Infrastructure.Repositories;

namespace Ordering.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection servicies
            , IConfiguration configuration)
        {
            servicies.AddDbContext<OrderContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString")));
            servicies.AddScoped<IOrderRepository, OrderRepository>();
            servicies.Configure<EmailSettings>(_ => configuration.GetSection("EmailSettings"));
            servicies.AddTransient<IEmailService, EmailService>();
            return servicies;
        }
    }
}