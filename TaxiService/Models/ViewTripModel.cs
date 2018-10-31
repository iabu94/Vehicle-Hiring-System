﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiService.Models
{
    public class ViewTripModel
    {
        public int tripId { get; set; }
        public int CarAmount { get; set; }
        public int VanAmount { get; set; }
        public int BusAmount { get; set; }

        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public int VehicleType { get; set; }
        public int TripDays { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
    }
}