using System.ComponentModel.DataAnnotations;
using TestAppUNS.Enums;

namespace TestAppUNS.Models
{
    public class OrderModel
    {
        public int Id { get; set; }

        [MinLength(5, ErrorMessage = "Order name must be at least 5 characters.")]
        public string? Name { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Order amount must be non-negative.")]
        public int Amount { get; set; }

        public DateTime CreationTime { get; set; }

        public DeliveryMethods DeliveryMethod { get; set; }

        public OrderStatuses Status { get; set; }
    }
}
