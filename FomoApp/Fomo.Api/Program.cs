using Fomo.Application.Services;
using Fomo.Infrastructure.ExternalServices;
using Fomo.Infrastructure.Persistence;
using Fomo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<IExternalApiHelper, ExternalApiHelper>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = $"https://{builder.Configuration["Auth0:Domain"]}/";
    options.Audience = builder.Configuration["Auth0:Audience"];
});

builder.Services.AddAuthorization();

builder.Services.AddDbContext<EFCoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["ConnectionString:EFCoreDBConnection"]);
});

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

builder.Services.Configure<TwelveData>(builder.Configuration.GetSection("TwelveData"));

builder.Services.AddScoped<IExternalApiHelper, ExternalApiHelper>();

builder.Services.AddScoped<ITwelveDataService, TwelveDataService>();

builder.Services.AddScoped<IIndicatorService, IndicatorService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ITradeResultRepository, TradeResultRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
