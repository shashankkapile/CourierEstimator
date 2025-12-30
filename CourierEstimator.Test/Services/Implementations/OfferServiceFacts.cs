using CourierEstimator.Core.Models;
using CourierEstimator.Core.Services.Implementations;
using Microsoft.Extensions.Options;

namespace CourierEstimator.Test.Services.Implementations
{
    public class OfferServiceFacts
    {
        private OfferService CreateService(List<Offer> offers)
        {
            var settings = Options.Create(new OfferSettings { Offers = offers });
            return new OfferService(settings);
        }

        [Fact]
        public void CanApplyOffer_ReturnsTrue_IfWeightAndDistanceValid()
        {
            var offer = new Offer
            {
                Code = "OFR001",
                MinWeight = 50,
                MaxWeight = 100,
                MinDistance = 30,
                MaxDistance = 150
            };

            var package = new Package
            {
                Id = "PKG1",
                Weight = 75,
                Distance = 100,
                OfferCode = "OFR001"
            };

            var service = CreateService(new List<Offer> { offer });
            var result = service.CanApplyOffer(package, offer);
            Assert.True(result);
        }

        [Fact]
        public void CanApplyOffer_ReturnsFalse_IfWeightIsInvalid()
        {
            var offer = new Offer
            {
                Code = "OFR001",
                MinWeight = 50,
                MaxWeight = 100,
                MinDistance = 30,
                MaxDistance = 150
            };

            var package = new Package
            {
                Id = "PKG2",
                Weight = 120,
                Distance = 100,
                OfferCode = "OFR001"
            };

            var service = CreateService(new List<Offer> { offer });
            var result = service.CanApplyOffer(package, offer);
            Assert.False(result);
        }

        [Fact]
        public void GetOfferByCode_ReturnsOffer_IfCodeExists()
        {
            var offer = new Offer { Code = "OFR001", MinWeight = 10, MaxWeight = 100, MinDistance = 10, MaxDistance = 200 };
            var service = CreateService(new List<Offer> { offer });

            var result = service.GetOfferByCode("OFR001");
            Assert.NotNull(result);
            Assert.Equal("OFR001", result.Code);
        }

        [Fact]
        public void GetOfferByCode_ReturnsNull_IfCodeDoesNotExist()
        {
            var service = CreateService(new List<Offer>());
            var result = service.GetOfferByCode("MyCode");
            Assert.Null(result);
        }

    }
}
