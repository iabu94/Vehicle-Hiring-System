//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaxiService.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class DriverDetailsTable
    {
        public int DriverID { get; set; }
        public string DriverCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Mobile { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string CurrentLocation { get; set; }
        public string UserImageUrl { get; set; }
        public Nullable<System.DateTime> RegisteredDate { get; set; }
        public string VehicleModel { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleYear { get; set; }
        public string VehicleColor { get; set; }
        public string VehicleNumber { get; set; }
        public string LicenseNo { get; set; }
        public int Status { get; set; }
        public int IsOnline { get; set; }
        public Nullable<int> IsDeleted { get; set; }
        public Nullable<int> VehicleType { get; set; }
    }
}
