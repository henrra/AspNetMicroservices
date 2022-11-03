using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Prendre en compte les fichiers de configuration ocelot.{env}.json
builder.Configuration.AddOcelot(builder.Environment);

// Ajout services
builder.Services.AddOcelot();

var app = builder.Build();
app.UseOcelot();

app.MapGet("/", () => "Hello World!");

app.Run();
