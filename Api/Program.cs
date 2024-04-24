using Api.Data;
using Api.Services.Supplier;
using Api.Services.WebScraper;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var allowedOrigins = "_allowedOrigins";
var builder = WebApplication.CreateBuilder(args);
{
    // Adding data persistence
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<DataContext>(
        options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
    );

    builder.Services.AddCors(options => {
        options.AddPolicy(
            name: allowedOrigins, policy => {
                policy.WithOrigins("http://localhost:5175").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
            }
        );
    });

    // Requests limiter
    builder.Services.AddMemoryCache();
    builder.Services.AddSingleton<IClientPolicyStore, MemoryCacheClientPolicyStore>();
    builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
    builder.Services.Configure<ClientRateLimitOptions>(
        options => {
            options.GeneralRules =
            [
                new RateLimitRule
                {
                    Endpoint = "*",
                    Period = "1m",
                    Limit = 20,
                }
            ];
        }
    );
    builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
    builder.Services.AddInMemoryRateLimiting();

    builder.Services.AddAuthorization();
    builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<DataContext>();
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();
    builder.Services.AddSingleton<IWebScraper, WebScraper>();
    builder.Services.AddScoped<ISupplierService, SupplierService>();
}
var app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.MapIdentityApi<IdentityUser>();
    app.UseExceptionHandler("/error");
    app.UseClientRateLimiting();
    app.UseHttpsRedirection();
    app.MapControllers();
    app.UseCors(allowedOrigins);
    app.Run();

}