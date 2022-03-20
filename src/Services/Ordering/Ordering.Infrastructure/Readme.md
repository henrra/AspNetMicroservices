### EF Migration
$ dotnet ef migrations add InitialCreate --project Services/Ordering.Infrastructure/Ordering.Infrastructure.csproj Build started...
```
It was not possible to find any compatible framework version
The framework 'Microsoft.NETCore.App', version '2.0.0' was not found.
- The following frameworks were found:
  5.0.8 at [/usr/share/dotnet/shared/Microsoft.NETCore.App]

You can resolve the problem by installing the specified framework and/or SDK.

The specified framework can be found at:
- https://aka.ms/dotnet-core-applaunch?framework=Microsoft.NETCore.App&framework_version=2.0.0&arch=x64&rid=manjaro-x64
```
=> Add package Microsoft.EntityFrameworkCore.Tools to Ordering.Infrastructure and Ordering.API projects

```
Unable to create an object of type 'OrderContext'. For the different patterns supported at design time, see https://go.microsoft.com/fwlink/?linkid=851728
````

=> Use command line : dotnet ef migrations add InitialCreate  --startup-project Services/Ordering.API/Ordering.API.csproj  --project Services/Ordering.Infrastructure/Ordering.Infrastructure.csproj --verbose

```
...System.ArgumentException: Cannot instantiate implementation type 'Ordering.Infrastructure.Repositories.RepositoryBase`1[T]' for service type 'Ordering.Application.Contracts.Persistence.IAsyncRepository`1[T]'.
...An error occurred while accessing the Microsoft.Extensions.Hosting services.
...No application service provider was found.

...Unable to resolve service for type 'Microsoft.EntityFrameworkCore.DbContextOptions`1[Ordering.Infrastructure.Persistence.OrderContext]' while attempting to activate 'Ordering.Infrastructure.Persistence.OrderContext'.

Code:
RepositoryBase.cs
public abstract class RepositoryBase<T> : IAsyncRepository<T> where T : EntityBase 
{
    protected readonly OrderContext _dbContext;    
    protected RepositoryBase(OrderContext dbContext)
    {
        _dbContext = dbContext;
    }
}

Services registration:
servicies.AddDbContext<OrderContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString")));
servicies.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
servicies.AddScoped<IOrderRepository, OrderRepository>();
```
=> Do not register RepositoryBase<>
