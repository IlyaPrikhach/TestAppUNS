using TestAppUNS.Servicies.Interfaces;

public class BackgroundReportsService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public BackgroundReportsService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var reportsService = scope.ServiceProvider.GetRequiredService<IReportsService>();
            await reportsService.CreateReportAsync();

            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }
}