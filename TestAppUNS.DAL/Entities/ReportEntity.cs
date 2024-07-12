namespace TestAppUNS.DAL.Entities
{
    public class ReportEntity: IEntity
    {
        public int Id { get; set; }
        
        public int? WaitingForConfirmationOrders { get; set; }

        public int? ConfirmedOrders { get; set; }

        public int? InProgressOrders { get; set; }

        public int? ReadyForPickupOrders { get; set; }

        public int? ReadyOrders { get; set; }

        public int? ReadyOrdersAmount { get; set; }

        public DateTime? CreationDate {  get; set; }
    }
}
