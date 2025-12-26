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

        var lineOne = Console.ReadLine();
        if (lineOne == null)
        {
            Console.WriteLine("Invalid line one");
            return;
        }

        var lineOneItems = lineOne.Split(" ");
        if (lineOneItems.Length < 2)
        {
            Console.WriteLine("Invalid line one items");
            return;
        }

        decimal baseCost = decimal.Parse(lineOneItems[0]);
        int packageCount = int.Parse(lineOneItems[1]);

        var packages = ReadPackages(packageCount);
        if (!packages.Any())
        {
            Console.WriteLine("No packages found");
            return;
        }

        var services = new ServiceCollection();
        RegisterServices(services, configuration);

        var provider = services.BuildServiceProvider();
        var costSettings = provider.GetRequiredService<IOptions<CostSettings>>().Value;
        costSettings.BaseCost = baseCost;

        BeginCostEstimation(provider, packages);

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
            if (line.Length < 3)
            {
                Console.WriteLine("Invalid package line items");
                return packages;
            }

            packages.Add(new Package
            {
                Id = lineItems[0],
                Weight = decimal.Parse(lineItems[1]),
                Distance = decimal.Parse(lineItems[2]),
                OfferCode = lineItems.Length > 3 ? lineItems[3] : string.Empty
            });
        }
        return packages;
    }

    private static void RegisterServices(ServiceCollection services, IConfiguration configuration) {
        services.Configure<CostSettings>(configuration.GetSection("Costs"));
        services.Configure<OfferSettings>(configuration.GetSection("Offers"));
        services.AddSingleton<IOfferService, OfferService>();
        services.AddTransient<ICostEstimatorService, CostEstimatorService>();
    }

    private static void BeginCostEstimation(ServiceProvider provider, List<Package> packages)
    {
        Console.WriteLine("Estimating delivery cost");
        var costEstimatorService = provider.GetRequiredService<ICostEstimatorService>();
        foreach (var package in packages)
        {
            var packageCostResult = costEstimatorService.CalculateCost(package);
            Console.WriteLine($"{packageCostResult.Id} {packageCostResult.Discount} {packageCostResult.TotalCost}");
        }
    }
}