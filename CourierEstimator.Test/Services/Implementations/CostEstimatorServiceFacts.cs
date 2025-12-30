using CourierEstimator.Core.Models;
using CourierEstimator.Core.Services.Implementations;
using CourierEstimator.Core.Services.Interfaces;
using Microsoft.Extensions.Options;
using Moq;

namespace CourierEstimator.Test.Services.Implementations
{
    public class CostEstimatorServiceFacts
    {
        private CostEstimatorService CreateService(IOfferService offerService, decimal baseCost, decimal costPerKg, decimal costPerKm)
        {
            var settings = Options.Create(new CostSettings
            {
                BaseCost = baseCost,
                CostPerKg = costPerKg,
                CostPerKm = costPerKm
            });

            return new CostEstimatorService(offerService, settings);
        }

        [Fact]
        public void CalculateCost_NoOffer()
        {
            var mockOfferService = new Mock<IOfferService>();
            var service = CreateService(mockOfferService.Object, baseCost: 100, costPerKg: 10, costPerKm: 5);

            var package = new Package
            {
                Id = "PKG1",
                Weight = 10,
                Distance = 20,
                OfferCode = string.Empty
            };

            service.CalculateCost(package);

            var expectedDeliveryCost = 100 + 10 * 10 + 20 * 5;
            Assert.Equal(expectedDeliveryCost, package.DeliveryCost);
            Assert.Equal(0, package.Discount);
            Assert.Equal(300, package.TotalCost);
        }

        [Fact]
        public void CalculateCost_WithValidOffer()
        {
            var offer = new Offer { Code = "OFR001", DiscountPercentage = 10 };
            var mockOfferService = new Mock<IOfferService>();
            mockOfferService.Setup(s => s.GetOfferByCode("OFR001")).Returns(offer);
            mockOfferService.Setup(s => s.CanApplyOffer(It.IsAny<Package>(), offer)).Returns(true);

            var service = CreateService(mockOfferService.Object, baseCost: 50, costPerKg: 5, costPerKm: 2);

            var package = new Package
            {
                Id = "PKG2",
                Weight = 20,
                Distance = 30,
                OfferCode = "OFR001"
            };

            service.CalculateCost(package);

            var deliveryCost = 50 + 20 * 5 + 30 * 2;
            var discount = deliveryCost * 0.10m;
            var totalCost = deliveryCost - discount;
            Assert.Equal(deliveryCost, package.DeliveryCost);
            Assert.Equal((int)discount, package.Discount);
            Assert.Equal((int)totalCost, package.TotalCost);
        }

        [Fact]
        public void CalculateCost_WithInvalidOffer()
        {
            var offer = new Offer { Code = "OFR002", DiscountPercentage = 20 };
            var mockOfferService = new Mock<IOfferService>();
            mockOfferService.Setup(s => s.GetOfferByCode("OFR002")).Returns(offer);
            mockOfferService.Setup(s => s.CanApplyOffer(It.IsAny<Package>(), offer)).Returns(false);

            var service = CreateService(mockOfferService.Object, baseCost: 100, costPerKg: 10, costPerKm: 1);

            var package = new Package
            {
                Id = "PKG3",
                Weight = 5,
                Distance = 50,
                OfferCode = "OFR002"
            };

            service.CalculateCost(package);

            var deliveryCost = 100 + 5 * 10 + 50 * 1;
            Assert.Equal(deliveryCost, package.DeliveryCost);
            Assert.Equal(0, package.Discount);
            Assert.Equal(200, package.TotalCost);
        }
    }
}