using CourierEstimator.Core.Models;
using CourierEstimator.Core.Services.Interfaces;

namespace CourierEstimator.Core.Services.Implementations
{
    public class TimeEstimatorService : ITimeEstimatorService
    {
        private readonly IOfferService _offerService;
        public TimeEstimatorService(IOfferService offerService)
        {
            _offerService = offerService;
        }

        public void CalculateTime(List<Package> packages, List<Vehicle> vehicles)
        {
            var vehicleQueue = new PriorityQueue<Vehicle, decimal>();
            var packageQueue = new PriorityQueue<Package, (decimal weight, decimal distance)>();

            foreach (var package in packages)
            {
                packageQueue.Enqueue(package, (-package.Weight, package.Distance));
            }

            foreach (var vehicle in vehicles)
            {
                vehicleQueue.Enqueue(vehicle, vehicle.NextAvailableTime);
            }

            while (packageQueue.Count > 0)
            {
                var vehicle = vehicleQueue.Dequeue();
                var tripStartTime = vehicle.NextAvailableTime;
                Console.WriteLine("Vehicle " + vehicle.Id + " at: " + tripStartTime);

                var reschedulePackages = new List<Package>();
                var maxTripTime = 0m;
                var remainingCapacity = vehicle.MaxCapacity;

                while (packageQueue.Count > 0)
                {
                    var package = packageQueue.Dequeue();

                    if (package.Weight > remainingCapacity)
                    {
                        reschedulePackages.Add(package);
                        continue;
                    }

                    remainingCapacity -= package.Weight;

                    var time = package.Distance / vehicle.MaxSpeed;
                    maxTripTime = Math.Max(time, maxTripTime);

                    Console.WriteLine($"Adding {package.Id} {package.Distance}/{vehicle.MaxSpeed} : {time}");
                    package.VehicleId = vehicle.Id;
                    package.DeliveryTime = tripStartTime + time;

                    if (remainingCapacity == 0)
                        break;
                }

                foreach (var package in reschedulePackages)
                {
                    packageQueue.Enqueue(package, (-package.Weight, package.Distance));
                }

                vehicle.NextAvailableTime += 2 * maxTripTime;
                vehicleQueue.Enqueue(vehicle, vehicle.NextAvailableTime);
            }
        }

    }
}
