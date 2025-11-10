using System.Diagnostics;
using Chapter02_Architecture;

var builder = WebApplication.CreateBuilder(args);

// look for MyConfigurationEntry in the configuration
var providers = (builder.Configuration as IConfigurationRoot).Providers.ToList();
var value = builder.Configuration.GetValue<int>("MyConfigurationEntry");
Debugger.Break();

// register greeting service
builder.Services.AddSingleton<IGreetingService, GreetingService>();



var app = builder.Build();

app.MapGet("/{name}", (string name, IGreetingService greetingService, ILogger<Program> logger) =>
{
	logger.LogInformation("Hello");
	return greetingService.Greet(name);
});

app.Run();