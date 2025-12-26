using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierEstimator.Core.Models
{
    public class CostSettings
    {
        public decimal BaseCost { get; set; }
        public decimal CostPerKg { get; set; }
        public decimal CostPerKm { get; set; }

    }
}
