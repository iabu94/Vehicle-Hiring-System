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
    
    public partial class SellingVehicleDetail
    {
        public int SellingID { get; set; }
        public string SellingHead { get; set; }
        public int MileageKm { get; set; }
        public string Price { get; set; }
        public string VehicleDescription { get; set; }
        public string VehicleBrand { get; set; }
        public string VehicleModel { get; set; }
        public Nullable<int> VehicleModelYear { get; set; }
        public string Condition { get; set; }
        public string EngineCapacity { get; set; }
        public Nullable<int> ContactNumber { get; set; }
        public Nullable<System.DateTime> UploadedDate { get; set; }
    }
}
