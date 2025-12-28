namespace CourierEstimator.Core.Models
{
    public class Package
    {
        public string Id { get; set; }
        public decimal Weight { get; set; }
        public decimal Distance { get; set; }
        public string OfferCode { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalCost { get; set; }

        public int VehicleId { get; set; }
        public decimal DeliveryTime { get; set; }
    }

}
