using CourierEstimator.Core.Models;

namespace CourierEstimator.Core.Services.Interfaces
{
    public interface ICostEstimatorService
    {
        public void CalculateCost(Package package);
    }
}
