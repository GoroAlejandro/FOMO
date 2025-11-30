using Fomo.Api.Helpers;
using Fomo.Application.Services;
using Fomo.Infrastructure.ExternalServices.StockService;
using Fomo.Infrastructure.ExternalServices.MailService;
using Fomo.Infrastructure.Persistence;
using Fomo.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Fomo.Infrastructure.ExternalServices.MailService.Alerts;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddHttpClient<IExternalApiHelper, ExternalApiHelper>();

var MyPolicy = "AllowFrontend";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyPolicy,
    policy =>
    {
        policy
            .WithOrigins($"{builder.Configuration["FrontendUrl:Url"]}")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

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

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IExternalApiHelper, ExternalApiHelper>();

builder.Services.AddScoped<ITwelveDataService, TwelveDataService>();

builder.Services.AddScoped<IIndicatorService, IndicatorService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ITradeResultRepository, TradeResultRepository>();

builder.Services.AddScoped<IUserValidateHelper, UserValidateHelper>();

builder.Services.AddScoped<ITradeResultValidateHelper, TradeResultValidateHelper>();

builder.Services.AddScoped<IStockRepository, StockRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IGenericAlert, GenericAlert>();

builder.Services.AddScoped<IAlertService, AlertService>();

builder.Services.AddScoped<IStockSyncService, StockSyncService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(MyPolicy);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var syncService = scope.ServiceProvider.GetRequiredService<IStockSyncService>();
    await syncService.SyncStockDb();
}

app.Run();
