using CourierEstimator.Core.Models;
using CourierEstimator.Core.Services.Implementations;
using CourierEstimator.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Starting the app");
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var packageMetadataLine = Console.ReadLine();
        if (packageMetadataLine == null)
        {
            Console.WriteLine("Invalid package metadata line");
            return;
        }

        var packageMetadata = packageMetadataLine.Split(" ");
        if (packageMetadata.Length < 2)
        {
            Console.WriteLine("Invalid package metadata");
            return;
        }

        decimal baseCost = decimal.Parse(packageMetadata[0]);
        int packageCount = int.Parse(packageMetadata[1]);

        var packages = ReadPackages(packageCount);
        if (!packages.Any())
        {
            Console.WriteLine("No packages found");
            return;
        }

        var vehicles = ReadVehicles();

        var services = new ServiceCollection();
        RegisterServices(services, configuration);

        var provider = services.BuildServiceProvider();
        var costSettings = provider.GetRequiredService<IOptions<CostSettings>>().Value;
        costSettings.BaseCost = baseCost;

        ProcessEstimations(provider, packages, vehicles);

        Console.Read();
    }

    private static List<Package> ReadPackages(int packageCount)
    {
        var packages = new List<Package>();
        for (int i = 0; i < packageCount; i++)
        {
            var line = Console.ReadLine();
            if (line == null)
            {
                Console.WriteLine("Invalid package line");
                return packages;
            }

            var lineItems = line.Split(' ');
            if (line.Length < 4)
            {
                Console.WriteLine("Invalid package line items");
                return packages;
            }

            packages.Add(new Package
            {
                Id = lineItems[0],
                Weight = decimal.Parse(lineItems[1]),
                Distance = decimal.Parse(lineItems[2]),
                OfferCode = lineItems[3]
            });
        }
        return packages;
    }
    private static List<Vehicle> ReadVehicles()
    {
        var vehicles = new List<Vehicle>();
        var vehicleMetadataLine = Console.ReadLine();
        if (vehicleMetadataLine == null)
        {
            Console.WriteLine("Invalid vehicle metadata line");
            return vehicles;
        }

        var vehicleMetadata = vehicleMetadataLine.Split(" ");
        if (vehicleMetadata.Length < 3)
        {
            Console.WriteLine("Invalid vehicle metadata");
            return vehicles;
        }

        var vehicleCount = int.Parse(vehicleMetadata[0]);
        var vehicleMaxSpeed = int.Parse(vehicleMetadata[1]);
        var vehicleMaxCapacity = int.Parse(vehicleMetadata[2]);

        for (int i = 0; i < vehicleCount; i++)
        {
            vehicles.Add(new Vehicle
            {
                Id = i+1,
                MaxCapacity = vehicleMaxCapacity,
                MaxSpeed = vehicleMaxSpeed,
                NextAvailableTime = 0m,
            });
        }
        return vehicles;
    }

    private static void RegisterServices(ServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CostSettings>(configuration.GetSection("Costs"));
        services.Configure<OfferSettings>(configuration.GetSection("Offers"));
        services.AddSingleton<IOfferService, OfferService>();
        services.AddTransient<ICostEstimatorService, CostEstimatorService>();
        services.AddTransient<ITimeEstimatorService, TimeEstimatorService>();
    }

    private static void ProcessEstimations(ServiceProvider provider, List<Package> packages, List<Vehicle> vehicles)
    {
        Console.WriteLine("Estimating delivery cost");
        var costEstimatorService = provider.GetRequiredService<ICostEstimatorService>();
        foreach (var package in packages)
        {
            costEstimatorService.CalculateCost(package);
            Console.WriteLine($"{package.Id} {package.Discount} {package.TotalCost}");
        }

        if (vehicles.Any())
        {
            var timeEstimatorService = provider.GetRequiredService<ITimeEstimatorService>();
            Console.WriteLine("Estimating delivery time");
            timeEstimatorService.CalculateTime(packages, vehicles);
            foreach (var package in packages)
            {
                Console.WriteLine($"{package.Id} {package.Discount} {package.TotalCost} {package.DeliveryTime} {package.VehicleId}");
            }
        }
    }
}