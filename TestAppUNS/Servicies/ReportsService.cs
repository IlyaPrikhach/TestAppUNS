using Microsoft.EntityFrameworkCore;
using TestAppUNS.DAL.Entities;
using TestAppUNS.DAL.Enums;
using TestAppUNS.DAL.Repositories.Interfaces;
using TestAppUNS.Servicies.Interfaces;

namespace TestAppUNS.Servicies
{
    public class ReportsService: IReportsService
    {
        private readonly IDbRepository _dbRepository;
        private readonly ILogger<IReportsService> _logger;

        public ReportsService(IDbRepository dbRepository, ILogger<IReportsService> logger)
        {
            _dbRepository = dbRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<OrderEntity>> GetAllOrders()
        {
            var orders = await _dbRepository.GetAll<OrderEntity>().ToListAsync();

            if (orders == null || !orders.Any())
            {
                throw new KeyNotFoundException("No orders were found");
            }

            return orders;
        }

        public async Task CreateReportAsync()
        {
            try
            {
                var orders = await GetAllOrders();

                var groupedOrders = orders.GroupBy(o => o.Status);

                var report = new ReportEntity
                {
                    WaitingForConfirmationOrders = groupedOrders.FirstOrDefault(g => g.Key == OrderStatuses.WaitingForConfirmation)?.Count() ?? 0,
                    ConfirmedOrders = groupedOrders.FirstOrDefault(g => g.Key == OrderStatuses.Confirmed)?.Count() ?? 0,
                    InProgressOrders = groupedOrders.FirstOrDefault(g => g.Key == OrderStatuses.InProgress)?.Count() ?? 0,
                    ReadyForPickupOrders = groupedOrders.FirstOrDefault(g => g.Key == OrderStatuses.ReadyForPickup)?.Count() ?? 0,
                    ReadyOrders = groupedOrders.FirstOrDefault(g => g.Key == OrderStatuses.Ready)?.Count() ?? 0,
                    ReadyOrdersAmount = groupedOrders
                        .Where(g => g.Key == OrderStatuses.Ready)
                        .Sum(g => g.Sum(o => o.Amount ?? 0)),
                    CreationDate = DateTime.UtcNow,
                };

                await _dbRepository.Add(report);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
