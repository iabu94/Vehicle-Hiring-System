using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiService.Models
{
    public class ViewAddsModel
    {
        public List<SellingVehicleDetail> VehicleDetails { get; set; }

        public List<SellingVehicleImageTable> VehicleImages { get; set; }
    }
}