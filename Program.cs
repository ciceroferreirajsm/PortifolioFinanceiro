using Microsoft.EntityFrameworkCore;
using PortifolioFinanceiro.Infrastructure;
using PortifolioFinanceiro.Repositories;
using PortifolioFinanceiro.Services;
using PortifolioFinanceiro.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Portfolio Financeiro API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Database configuration
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("PortifolioFinanceiroDb"));

// Repositories
builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IPortfolioRepository, PortfolioRepository>();
builder.Services.AddScoped<IPositionRepository, PositionRepository>();

// Services
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();
builder.Services.AddScoped<SeedDataService>();

var app = builder.Build();

// Configure the HTTP request pipeline.

// 1. Error handling middleware (deve ser o primeiro)
app.UseErrorHandling();

// 2. Swagger (antes dos middlewares de autenticação)
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Portfolio Financeiro API v1");
        c.RoutePrefix = string.Empty; 
    });
}

// 3. Authentication & Authorization
app.UseAuthorization();

// 4. Response formatting middleware (antes dos controllers)
app.UseResponseFormatting();

// 5. Controllers
app.MapControllers();

// Seed database
using (var scope = app.Services.CreateScope())
{
    try
    {
        var seedService = scope.ServiceProvider.GetRequiredService<SeedDataService>();
        await seedService.SeedDatabaseAsync();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database");
    }
}

app.Run();