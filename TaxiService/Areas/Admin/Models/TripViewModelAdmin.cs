using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaxiService.Models;

namespace TaxiService.Areas.Admin.Models
{
    public class TripViewModelAdmin
    {
        public TripsTable Trips { get; set; }
        public TripDetaisTable TripDetails { get; set; }
        public DriverDetailsTable Driver { get; set; }
        public RiderDetailsTable Rider { get; set; }
    }

    public class ApproveTripModel
    {
        public int DriverID { get; set; }
        public int TripID { get; set; }
    }
}