using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.Infrastructure.Persistence;

var host = CreateHostBuilder(args).Build();
using (var scope = host.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrderContext>();
    context.Database.Migrate();
}

host.Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
       Host.CreateDefaultBuilder(args)
           .ConfigureServices((hostContext, services) =>
           {
               var connectionString = hostContext.Configuration.GetConnectionString("OrderingConnectionString");
               services.AddDbContext<OrderContext>(opts => opts.UseSqlServer(connectionString));
           });