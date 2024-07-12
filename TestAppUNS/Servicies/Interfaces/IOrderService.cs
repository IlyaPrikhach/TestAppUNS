using TestAppUNS.Enums;
using TestAppUNS.Models;

namespace TestAppUNS.Servicies.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderModel>> GetOrders(int? id, string? name, DeliveryMethods? method, int page);

        OrderModel? GetOrder(int id);

        Task<int> CreateOrder(OrderModel order);

        Task UpdateOrder(OrderModel updatedOrder);

        Task DeleteOrder(int id);
    }
}
