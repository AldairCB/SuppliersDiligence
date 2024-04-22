using Api.Data;
using Api.Services.Supplier;
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
                policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
            }
        );
    });

    builder.Services.AddAuthorization();
    builder.Services.AddIdentityApiEndpoints<IdentityUser>().AddEntityFrameworkStores<DataContext>();
    
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();
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
    app.UseHttpsRedirection();
    app.MapControllers();
    app.UseCors(allowedOrigins);
    app.Run();

}