using CourierEstimator.Core.Models;

namespace CourierEstimator.Core.Services.Interfaces
{
    public interface ITimeEstimatorService
    {
        public void CalculateTime(List<Package> packages, List<Vehicle> vehicles);
    }
}
