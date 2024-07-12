using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using TestAppUNS.DAL;
using TestAppUNS.DAL.Repositories;
using TestAppUNS.DAL.Repositories.Interfaces;
using TestAppUNS.MappingProfiles;
using TestAppUNS.Servicies;
using TestAppUNS.Servicies.Interfaces;


var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddAutoMapper(typeof(TestProfile));

    builder.Services.AddTransient<IOrderService, OrderService>();
    builder.Services.AddTransient<IReportsService, ReportsService>();
    builder.Services.AddScoped<IDbRepository, DbRepository>();

    builder.Services.AddHostedService<BackgroundReportsService>();

    builder.Services.AddDbContext<DataContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
            assembly => assembly.MigrationsAssembly("TestAppUNS.DAL")));

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline. 
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}

