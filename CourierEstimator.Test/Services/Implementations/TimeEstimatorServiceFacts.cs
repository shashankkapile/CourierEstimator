using CourierEstimator.Core.Models;
using CourierEstimator.Core.Services.Implementations;
using CourierEstimator.Core.Services.Interfaces;
using Moq;

namespace CourierEstimator.Test.Services.Implementations
{
    public class TimeEstimatorServiceFacts
    {
        private readonly ITimeEstimatorService _timeEstimatorService;
        public TimeEstimatorServiceFacts()
        {
            _timeEstimatorService = new TimeEstimatorService();
        }


        [Fact]
        public void OneVehicle_OnePackage_OneShipment()
        {
            var packages = new List<Package>
            {
                new Package { Id = "PKG1", Weight = 5, Distance = 10 }
            };

            var vehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, MaxCapacity = 10, MaxSpeed = 5, NextAvailableTime = 0 }
            };

            _timeEstimatorService.CalculateTime(packages, vehicles);

            Assert.Equal(2, packages[0].DeliveryTime);
            Assert.Equal(1, packages[0].VehicleId);
            Assert.Equal(4, vehicles[0].NextAvailableTime);
        }

        [Fact]
        public void OneVehicle_TwoPackage_OneShipment()
        {
            var packages = new List<Package>
            {
                new Package { Id = "PKG1", Weight = 3, Distance = 10 },
                new Package { Id = "PKG2", Weight = 4, Distance = 20 }
            };

            var vehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, MaxCapacity = 10, MaxSpeed = 10, NextAvailableTime = 0 }
            };

            _timeEstimatorService.CalculateTime(packages, vehicles);

            Assert.Equal(1, packages[0].DeliveryTime);
            Assert.Equal(2, packages[1].DeliveryTime);
            Assert.Equal(4, vehicles[0].NextAvailableTime);
        }

        [Fact]
        public void OneVehicle_TwoPackage_TwoShipment()
        {
            var packages = new List<Package>
            {
                new Package { Id = "PKG1", Weight = 6, Distance = 10 },
                new Package { Id = "PKG2", Weight = 6, Distance = 15 }
            };

            var vehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, MaxCapacity = 10, MaxSpeed = 5, NextAvailableTime = 0 }
            };

            _timeEstimatorService.CalculateTime(packages, vehicles);

            Assert.Equal(2, packages[0].DeliveryTime);
            Assert.Equal(7, packages[1].DeliveryTime);
            Assert.Equal(10, vehicles[0].NextAvailableTime);
        }

        [Fact]
        public void TwoVehicles_ThreePackages_TwoShipments()
        {
            var packages = new List<Package>
            {
                new Package { Id = "PKG1", Weight = 4, Distance = 10 },
                new Package { Id = "PKG2", Weight = 5, Distance = 20 },
                new Package { Id = "PKG3", Weight = 6, Distance = 15 }
            };

            var vehicles = new List<Vehicle>
            {
                new Vehicle { Id = 1, MaxCapacity = 10, MaxSpeed = 10, NextAvailableTime = 0 },
                new Vehicle { Id = 2, MaxCapacity = 10, MaxSpeed = 5, NextAvailableTime = 0 }
            };

            _timeEstimatorService.CalculateTime(packages, vehicles);

            Assert.Equal(1, packages[0].VehicleId);
            Assert.Equal(1, packages[2].VehicleId);
            Assert.Equal(2, packages[1].VehicleId);
            Assert.Equal(1, packages[0].DeliveryTime);
            Assert.Equal(1.5m, packages[2].DeliveryTime);
            Assert.Equal(4, packages[1].DeliveryTime);
            Assert.Equal(3, vehicles[0].NextAvailableTime);
            Assert.Equal(8, vehicles[1].NextAvailableTime);
        }
    }
}
