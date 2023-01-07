using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Prendre en compte les fichiers de configuration ocelot.{env}.json
builder.Configuration.AddOcelot(builder.Environment);
var envName = builder.Environment.IsProduction() ? string.Empty : $".{builder.Environment.EnvironmentName}";
builder.Configuration.AddJsonFile($"ocelot{envName}.json", optional: false, reloadOnChange: false);

// Ajout services
builder.Services.AddOcelot();

var app = builder.Build();
app.UseOcelot().Wait();
app.Run();
