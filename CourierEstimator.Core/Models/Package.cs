using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierEstimator.Core.Models
{
    public class Package
    {
        public string Id { get; set; }
        public decimal Weight { get; set; }
        public decimal Distance { get; set; }
        public string OfferCode { get; set; }
    }

}
