using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiService.Models
{
    public class ConfirmedDetailViewModel
    {
        public string PickupLocation { get; set; }
        public DateTime PickupDateTime { get; set; }
        public string DropLocation { get; set; }
        public double EstimatedFare { get; set; }
        public string VehicleNumber { get; set; }
        public string VehicleBrand { get; set; }
        public int ContactNo { get; set; }
        public string Image { get; set; }
    }
}