using Fomo.Api.Helpers;
using Fomo.Application.Services;
using Fomo.Infrastructure.ExternalServices;
using Fomo.Infrastructure.Persistence;
using Fomo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
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

builder.Services.AddScoped<IUserValidateHelper, UserValidateHelper>();

builder.Services.AddScoped<ITradeResultValidateHelper, TradeResultValidateHelper>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
