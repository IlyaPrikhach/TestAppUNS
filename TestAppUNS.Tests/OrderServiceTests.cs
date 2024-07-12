using AutoMapper;
using Moq;
using TestAppUNS.DAL.Entities;
using TestAppUNS.DAL.Repositories.Interfaces;
using TestAppUNS.Enums;
using TestAppUNS.MappingProfiles;
using TestAppUNS.Models;
using TestAppUNS.Servicies;
using TestAppUNS.Servicies.Interfaces;

namespace TestAppUNS.Tests
{
    public class OrderServiceTests
    {
        private readonly IOrderService _orderService;
        private readonly Mock<IDbRepository> _dbRepositoryMock;
        private readonly IMapper _mapper;

        public OrderServiceTests()
        {
            _dbRepositoryMock = new Mock<IDbRepository>();
            _mapper = new MapperConfiguration(cfg => cfg.AddProfile<TestProfile>()).CreateMapper();

            _orderService = new OrderService(_dbRepositoryMock.Object, _mapper);
        }

        [Fact]
        public async Task GetOrders_Should_Return_Orders()
        {
            // Arrange
            var orders = new List<OrderEntity>
            {
                new OrderEntity { Id = 1, Name = "Order1", Amount = 100, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.HomeDelivery },
                new OrderEntity { Id = 2, Name = "Order2", Amount = 200, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.ParcelLocker },
                new OrderEntity { Id = 3, Name = "Order3", Amount = 300, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.PickupFromWarehouse }
            };

            _dbRepositoryMock.Setup(repo => repo.GetAll<OrderEntity>()).Returns(orders.AsQueryable());

            // Act
            var result = await _orderService.GetOrders(null, null, null, 1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Order1", result.First().Name);
        }

        [Fact]
        public async Task GetOrders_WithId_Should_Return_Order()
        {
            // Arrange
            var orders = new List<OrderEntity>
            {
                new OrderEntity { Id = 1, Name = "Order1", Amount = 100, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.HomeDelivery },
                new OrderEntity { Id = 2, Name = "Order2", Amount = 200, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.ParcelLocker },
                new OrderEntity { Id = 3, Name = "Order3", Amount = 300, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.PickupFromWarehouse }
            };

            _dbRepositoryMock.Setup(repo => repo.GetAll<OrderEntity>()).Returns(orders.AsQueryable());

            // Act
            var result = await _orderService.GetOrders(2, null, null, 1);

            // Assert
            Assert.Single(result);
            Assert.Equal(2, result.First().Id);
        }

        [Fact]
        public async Task GetOrders_WithName_Should_Return_Orders()
        {
            // Arrange
            var orders = new List<OrderEntity>
            {
                new OrderEntity { Id = 1, Name = "Order1", Amount = 100, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.HomeDelivery },
                new OrderEntity { Id = 2, Name = "Order2", Amount = 200, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.ParcelLocker },
                new OrderEntity { Id = 3, Name = "Order2", Amount = 300, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.PickupFromWarehouse }
            };

            _dbRepositoryMock.Setup(repo => repo.GetAll<OrderEntity>()).Returns(orders.AsQueryable());

            // Act
            var result = await _orderService.GetOrders(null, "Order2", null, 1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("Order2", result.First().Name);
        }

        [Fact]
        public async Task GetOrders_WithDeliveryMethod_Should_Return_Orders()
        {
            // Arrange
            var orders = new List<OrderEntity>
            {
                new OrderEntity { Id = 1, Name = "Order1", Amount = 100, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.HomeDelivery },
                new OrderEntity { Id = 2, Name = "Order2", Amount = 200, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.ParcelLocker },
                new OrderEntity { Id = 3, Name = "Order2", Amount = 300, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.HomeDelivery }
            };

            _dbRepositoryMock.Setup(repo => repo.GetAll<OrderEntity>()).Returns(orders.AsQueryable());

            // Act
            var result = await _orderService.GetOrders(null, null, DeliveryMethods.HomeDelivery, 1);

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal(DeliveryMethods.HomeDelivery, result.First().DeliveryMethod);
        }

        [Fact]
        public async Task GetOrders_WhenInvalidPage_Should_Throw_KeyNotFoundException()
        {
            // Arrange
            var orders = new List<OrderEntity>
            {
                new OrderEntity { Id = 1, Name = "Order1", Amount = 100, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.HomeDelivery },
                new OrderEntity { Id = 2, Name = "Order2", Amount = 200, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.ParcelLocker },
                new OrderEntity { Id = 3, Name = "Order2", Amount = 300, DeliveryMethod = TestAppUNS.DAL.Enums.DeliveryMethods.HomeDelivery }
            };

            _dbRepositoryMock.Setup(repo => repo.GetAll<OrderEntity>()).Returns(orders.AsQueryable());

            // Act && Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _orderService.GetOrders(null, null, null, 10));
        }

        public async Task GetOrders_WhenNoOrdersFound_Should_Throw_KeyNotFoundException()
        {
            // Arrange
            var orders = new List<OrderEntity>();

            _dbRepositoryMock.Setup(repo => repo.GetAll<OrderEntity>()).Returns(orders.AsQueryable());

            // Act && Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await _orderService.GetOrders(null, null, null, 1));
        }

        [Fact]
        public async Task CreateOrder_Should_Add_Order()
        {
            // Arrange
            var orderModel = new OrderModel { Name = "New Order", Amount = 150 };

            _dbRepositoryMock.Setup(repo => repo.Add(It.IsAny<OrderEntity>())).ReturnsAsync(3);

            // Act
            var orderId = await _orderService.CreateOrder(orderModel);

            // Assert
            Assert.Equal(3, orderId);
            _dbRepositoryMock.Verify(repo => repo.Add(It.IsAny<OrderEntity>()), Times.Once);
        }
    }
}