using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiService.Models
{
    public class HomeBookingModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public int MobileNumber { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime PickUpTime { get; set; }
        public int TripType { get; set; }
        public int VehicleType { get; set; }
        public float DistanceKm { get; set; }
        public int DurationMin { get; set; }
        public float HirePrice { get; set; }
        public int VehicleID { get; set; }
        public int UserID { get; set; }
    }
}