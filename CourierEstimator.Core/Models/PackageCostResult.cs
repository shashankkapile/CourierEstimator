namespace CourierEstimator.Core.Models
{
    public class PackageCostResult
    {
        public string Id { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalCost { get; set; }
    }
}
