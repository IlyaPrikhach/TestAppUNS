using TestAppUNS.DAL.Entities;

namespace TestAppUNS.Servicies.Interfaces
{
    public interface IReportsService
    {
        Task<IEnumerable<OrderEntity>> GetAllOrders();

        Task CreateReportAsync();
    }
}
