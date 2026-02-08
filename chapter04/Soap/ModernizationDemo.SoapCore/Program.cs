using ModernizationDemo.SoapCore;
using SoapCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSoapCore();
builder.Services.AddScoped<Products>();
builder.Services.AddScoped<Orders>();

var app = builder.Build();

app.UseRouting();

app.UseSoapEndpoint<Products>("/Products.asmx", new SoapEncoderOptions());
app.UseSoapEndpoint<Orders>("/Orders.asmx", new SoapEncoderOptions());

app.Run();
