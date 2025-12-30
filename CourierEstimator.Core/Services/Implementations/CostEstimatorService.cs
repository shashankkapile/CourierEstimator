using CourierEstimator.Core.Models;
using CourierEstimator.Core.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace CourierEstimator.Core.Services.Implementations
{
    public class CostEstimatorService : ICostEstimatorService
    {
        private readonly IOfferService _offerService;
        private readonly CostSettings _costSettings;
        public CostEstimatorService(IOfferService offerService, IOptions<CostSettings> costSettings)
        {
            _offerService = offerService;
            _costSettings = costSettings.Value;
        }

        public void CalculateCost(Package package)
        {
            try
            {
                var deliveryCost = _costSettings.BaseCost
                + (package.Weight * _costSettings.CostPerKg)
                + (package.Distance * _costSettings.CostPerKm);

                var discount = 0m;
                if (!string.IsNullOrEmpty(package.OfferCode))
                {
                    var offer = _offerService.GetOfferByCode(package.OfferCode);
                    if (offer != null && _offerService.CanApplyOffer(package, offer))
                    {
                        discount = deliveryCost * (offer.DiscountPercentage / 100);
                    }
                }

                package.DeliveryCost = deliveryCost;
                package.Discount = (int)discount;
                package.TotalCost = (int)(deliveryCost - discount);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
