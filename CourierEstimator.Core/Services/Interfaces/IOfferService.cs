using CourierEstimator.Core.Models;

namespace CourierEstimator.Core.Services.Interfaces
{
    public interface IOfferService
    {
        public Offer? GetOfferByCode(string code);
        public bool CanApplyOffer(Package package, Offer offer);
    }
}
