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

        var packages = new List<Package>();
        for (int i = 0; i < packageCount; i++)
        {
            var line = Console.ReadLine();
            if (line == null)
            {
                Console.WriteLine("Invalid input line");
                return;
            }

            var lineItems = line.Split(' ');
            if (line.Length < 3)
            {
                Console.WriteLine("Invalid input line items");
                return;
            }

            packages.Add(new Package
            {
                Id = lineItems[0],
                Weight = decimal.Parse(lineItems[1]),
                Distance = decimal.Parse(lineItems[2]),
                OfferCode = lineItems.Length>3 ? lineItems[3]: string.Empty
            });
        }

        Console.WriteLine("Input parsed");

        var services = new ServiceCollection();
        services.Configure<CostSettings>(configuration.GetSection("Costs"));
        services.Configure<OfferSettings>(configuration.GetSection("Offers"));
        services.AddSingleton<IOfferService, OfferService>();
        services.AddTransient<ICostEstimatorService, CostEstimatorService>();

        var provider = services.BuildServiceProvider();
        var costSettings = provider.GetRequiredService<IOptions<CostSettings>>().Value;
        costSettings.BaseCost = baseCost;

        Console.WriteLine("Estimating delivery cost");
        var costEstimatorService = provider.GetRequiredService<ICostEstimatorService>();
        foreach(var package in packages)
        {
            var packageCostResult = costEstimatorService.CalculateCost(package);
            Console.WriteLine($"{packageCostResult.Id} {packageCostResult.Discount} {packageCostResult.TotalCost}");
        }

        Console.Read();
    }
}