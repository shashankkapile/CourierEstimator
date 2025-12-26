using CourierEstimator.Core.Models;
using CourierEstimator.Core.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace CourierEstimator.Core.Services.Implementations
{
    public class OfferService : IOfferService
    {
        private readonly List<Offer> _offers;
        public OfferService(IOptions<OfferSettings> offerSettings)
        {
            _offers = offerSettings.Value.Offers;
        }

        public bool CanApplyOffer(Package package, Offer offer)
        {
            var validWeight = offer.MinWeight <= package.Weight
                && package.Weight <= offer.MaxWeight;
            var validDistance = offer.MinDistance <= package.Distance
                && package.Distance <= offer.MaxDistance;

            return validWeight && validDistance;
        }

        public Offer? GetOfferByCode(string code)
        {
            return _offers.FirstOrDefault(x => x.Code == code);
        }
    }
}
