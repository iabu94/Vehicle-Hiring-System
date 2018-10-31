using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiService.Models
{
    public class AddDriverViewModel
    {
        public string DriverCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string NICNo { get; set; }
        public string Gender { get; set; }
        public System.DateTime DOB { get; set; }
        public string LicenseNo { get; set; }
        public int UserLoginID { get; set; }
        public int ContactNumber { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
    }
}