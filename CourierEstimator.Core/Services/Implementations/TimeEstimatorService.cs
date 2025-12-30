using CourierEstimator.Core.Helpers;
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
            var pendingPackages = new List<Package>(packages);
            while (pendingPackages.Count > 0)
            {
                var vehicle = vehicles.OrderBy(v => v.NextAvailableTime).First();

                var capacity = vehicle.MaxCapacity;

                var dp = Tabulation(pendingPackages, capacity);

                var pickedPackages = GetPickedPackages(dp, pendingPackages, capacity);

                var tripStartTime = vehicle.NextAvailableTime;
                var maxTripTime = 0m;

                foreach (var pkg in pickedPackages)
                {
                    var travelTime = pkg.Distance / vehicle.MaxSpeed;
                    var deliveryTime = tripStartTime + travelTime;
                    pkg.DeliveryTime = RoundHelper.Round(deliveryTime);
                    pkg.VehicleId = vehicle.Id;

                    maxTripTime = Math.Max(maxTripTime, travelTime);
                }
                var returnTime = 2 * RoundHelper.Round(maxTripTime);
                vehicle.NextAvailableTime = RoundHelper.Round(vehicle.NextAvailableTime + returnTime);
                //Console.WriteLine("Next--> " + vehicle.NextAvailableTime);
                pendingPackages.RemoveAll(p => pickedPackages.Contains(p));
            }
        }

    
        private TabulationResult Better(TabulationResult a, TabulationResult b)
        {
            if (a.PackageCount != b.PackageCount)
                return a.PackageCount > b.PackageCount ? a : b;

            if (a.TotalWeight != b.TotalWeight)
                return a.TotalWeight > b.TotalWeight ? a : b;

            return a.MaxDistance < b.MaxDistance ? a : b;
        }


        private TabulationResult[,] Tabulation(List<Package> packages, decimal capacity)
        {
            int cap = (int)capacity;
            var dp = new TabulationResult[packages.Count + 1, cap + 1];

            for (int i = 0; i <= packages.Count; i++)
                for (int w = 0; w <= cap; w++)
                    dp[i, w] = new TabulationResult();

            for (int i = 1; i <= packages.Count; i++)
            {
                var pkg = packages[i - 1];

                for (int w = 0; w <= cap; w++)
                {
                    var nonPick = dp[i - 1, w];
                    TabulationResult pick = null;

                    if (pkg.Weight <= w)
                    {
                        var prev = dp[i - 1, w - (int)pkg.Weight];
                        pick = new TabulationResult
                        {
                            PackageCount = prev.PackageCount + 1,
                            TotalWeight = prev.TotalWeight + pkg.Weight,
                            MaxDistance = Math.Max(prev.MaxDistance, pkg.Distance)
                        };
                    }

                    dp[i, w] = pick == null ? nonPick : Better(pick, nonPick);
                }
            }
            return dp;
        }

        private List<Package> GetPickedPackages(TabulationResult[,] dp, List<Package> packages, decimal capacity)
        {
            //Console.WriteLine("Generating picked packages");
            var picked = new List<Package>();
            int i = packages.Count;
            int w = (int)capacity;

            while (i > 0 && w > 0)
            {
                if (dp[i, w].PackageCount == dp[i - 1, w].PackageCount &&
                    dp[i, w].TotalWeight == dp[i - 1, w].TotalWeight)
                {
                    i--;
                }
                else
                {
                    var pkg = packages[i - 1];
                    picked.Add(pkg);
                    w = w - (int)pkg.Weight;
                    i--;
                }
            }

            return picked;
        }
    }
}
