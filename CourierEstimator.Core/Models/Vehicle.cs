using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierEstimator.Core.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public decimal MaxSpeed { get; set; }
        public decimal MaxCapacity { get; set; }
        public decimal NextAvailableTime { get; set; }
    }

}
