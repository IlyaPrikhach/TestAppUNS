using AutoMapper;
using TestAppUNS.DAL.Entities;
using TestAppUNS.DAL.Repositories.Interfaces;
using TestAppUNS.Enums;
using TestAppUNS.Models;
using TestAppUNS.Servicies.Interfaces;

namespace TestAppUNS.Servicies
{
    public class OrderService: IOrderService
    {
        private readonly IDbRepository _dbRepository;
        private readonly IMapper _mapper;

        public OrderService(IDbRepository dbRepository, IMapper mapper)
        {
            _dbRepository = dbRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<OrderModel>> GetOrders(int? id, string? name, DeliveryMethods? method, int page)
        {
            var orders = _dbRepository.GetAll<OrderEntity>();
            
            int pageSize = 2;

            if (id.HasValue)
            {
                orders = orders.Where(o => o.Id == id);
            }
            if (!string.IsNullOrEmpty(name))
            {
                orders = orders.Where(o => o.Name == name);
            }
            if (method.HasValue)
            {
                var d = (TestAppUNS.DAL.Enums.DeliveryMethods)method;
                orders = orders.Where(o => o.DeliveryMethod == d);
            }
            if (!orders.Any())
            {
                throw new KeyNotFoundException("No orders found");
            }

            var count = await Task.FromResult(orders.Count());
            var totalPagesCount = (int)Math.Ceiling((double)count / pageSize);

            if (page < 0 || page > totalPagesCount)
            {
                
                throw new KeyNotFoundException("This page does not exist");
            }

            return await Task.FromResult(orders.Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(order => _mapper.Map<OrderModel>(order))
                .ToList());
        }

        public OrderModel? GetOrder(int id)
        {
            var entity = _dbRepository.Get<OrderEntity>(x => x.Id == id).FirstOrDefault();

            if (entity == null)
            {
                return null;
            }

            var result = _mapper.Map<OrderModel>(entity);
            return result;
        }

        public async Task<int> CreateOrder(OrderModel order)
        {
            // Validate order fields
            if (string.IsNullOrEmpty(order.Name) || order.Name.Length < 5)
            {
                throw new ArgumentException("Invalid order name.");
            }
            if (order.Amount < 0)
            {
                throw new ArgumentException("Order amount must be non-negative.");
            }

            order.CreationTime = DateTime.UtcNow;
            return await _dbRepository.Add(_mapper.Map<OrderEntity>(order));
        }

        public async Task UpdateOrder(OrderModel updatedOrder)
        {
            var existingOrder = _mapper.Map<OrderEntity>(updatedOrder);

            await _dbRepository.Update(existingOrder);
        }

        public async Task DeleteOrder(int id)
        {
            var entity = _dbRepository.Get<OrderEntity>(x => x.Id == id).FirstOrDefault();

            if (entity == null)
            {
                throw new KeyNotFoundException("No orders found");
            }

            await _dbRepository.Delete(entity);
        }
    }
}
