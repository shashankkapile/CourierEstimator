namespace CourierEstimator.Core.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public decimal MaxSpeed { get; set; }
        public int MaxCapacity { get; set; }
        public decimal NextAvailableTime { get; set; }
    }

}
