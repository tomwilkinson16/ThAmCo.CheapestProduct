using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using ThAmCo.CheapestProduct.Services.CheapestProducts;
using ThAmCo.CheapestProducts.Services.CheapestProduct;
// using ThAmCo.CheapestProduct.Services.CheapestProducts


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders().AddConsole().AddDebug().AddAzureWebAppDiagnostics();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Auth:Authority"];
        options.Audience = builder.Configuration["Auth:Audience"];
    });
builder.Services.AddAuthorization();
 
builder.Services.AddMemoryCache();

if(builder.Environment.IsDevelopment())
{
    // builder.Services.AddSingleton<ILowestPriceService, LowestPriceServiceFake>();
    builder.Services.AddHttpClient<ILowestPriceService, LowestProducts>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["LowestProducts:Uri"]);
    });

}
else
{
    builder.Services.AddHttpClient<ILowestPriceService, LowestProducts>(client =>
    {
        client.BaseAddress = new Uri(builder.Configuration["LowestProducts:Uri"]);
    });
}

var app = builder.Build();
 
if (app.Environment.IsDevelopment())
{
     app.UseSwagger();
     app.UseSwaggerUI();
}
 
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
 
app.MapControllers();
 
app.Run();

public record WeatherForecast
{
    public DateOnly Date { get; init; }
    public int TemperatureC { get; init; }
    public string? Summary { get; init; }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

