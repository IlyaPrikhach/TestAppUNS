using TestAppUNS.DAL.Enums;

namespace TestAppUNS.DAL.Entities
{
    public class OrderEntity: IEntity
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int? Amount { get; set; }

        public DateTime? CreationTime { get; set; }

        public DeliveryMethods? DeliveryMethod { get; set; }

        public OrderStatuses? Status { get; set; }
    }
}
