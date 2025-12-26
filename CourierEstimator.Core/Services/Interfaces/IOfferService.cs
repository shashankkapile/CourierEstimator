using CourierEstimator.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierEstimator.Core.Services.Interfaces
{
    public interface IOfferService
    {
        public Offer? GetOfferByCode(string code);
        public bool CanApplyOffer(Package package, Offer offer);
    }
}
