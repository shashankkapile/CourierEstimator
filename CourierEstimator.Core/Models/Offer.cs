namespace CourierEstimator.Core.Models
{
    public class Offer
    {
        public string Code { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal MinWeight { get; set; }
        public decimal MaxWeight { get; set; }
        public decimal MinDistance { get; set; }
        public decimal MaxDistance { get; set; }
    }
}
