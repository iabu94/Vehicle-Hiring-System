using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiService.Models;

namespace TaxiService.Areas.Admin.Controllers
{
    public class VehicleController : Controller
    {

        // GET: Admin/Vehicle
        //public ActionResult TukTuk()
        //{
        //    using (TaxiServiceEntities db = new TaxiServiceEntities())
        //    {
        //        ViewModels ListVehicle = new ViewModels();
        //        ListVehicle.VehicleList = db.Vehicles.ToList();

        //        return View(ListVehicle);
        //    }

        //}

        public ActionResult AddTukTuk()
        {
            return PartialView("AddVehiclePV");
        }

        //[HttpPost]
        //public ActionResult AddVehicle(Vehicle model)
        //{
        //    using (TaxiServiceEntities db = new TaxiServiceEntities())
        //    {
        //        model.IsAvailable = "Yes";

        //        db.Vehicles.Add(model);
        //        db.SaveChanges();

        //        TempData["Success"] = "Added Successfully!";

        //        return RedirectToAction("TukTuk", "Vehicle");
        //    }
        //}

        //public ActionResult ViewVehicle(int vehicleId)
        //{
        //    using (TaxiServiceEntities db = new TaxiServiceEntities())
        //    {
        //        Vehicle vehicle = db.Vehicles.Where(o => o.VehicleID == vehicleId).FirstOrDefault();
        //        return PartialView("ViewVehiclePV", vehicle);
        //    }
        //}

        //public ActionResult EditVehicle(int vehicleId)
        //{
        //    using (TaxiServiceEntities db = new TaxiServiceEntities())
        //    {
        //        Vehicle vehicle = db.Vehicles.Where(o => o.VehicleID == vehicleId).FirstOrDefault();
        //        return PartialView("EditVehiclePV", vehicle);
        //    }
        //}

        //[HttpPost]
        //public ActionResult EditVehicle(Vehicle vehicle)
        //{
        //    using (TaxiServiceEntities db = new TaxiServiceEntities())
        //    {
        //        vehicle.IsAvailable = "Yes";
        //        db.Entry(vehicle).State = System.Data.Entity.EntityState.Modified;
        //        db.SaveChanges();
                
        //        return RedirectToAction("TukTuk");
        //    }
            
        //}

        //public ActionResult DeleteVehicle(int vehicleId)
        //{
        //    using (TaxiServiceEntities db = new TaxiServiceEntities())
        //    {
        //        Vehicle vehicle = db.Vehicles.Where(o => o.VehicleID == vehicleId).FirstOrDefault();
        //        db.Vehicles.Remove(vehicle);
        //        db.SaveChanges();
        //        return Json("success", JsonRequestBehavior.AllowGet);
        //    }
            
        //}
    }
}