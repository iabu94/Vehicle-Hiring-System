using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiService.Areas.Admin.Models;
using TaxiService.Models;
using static TaxiService.Common.Enum;

namespace TaxiService.Areas.Admin.Controllers
{
    public class TripController : Controller
    {
        private TaxiServiceEntities context = new TaxiServiceEntities();

        public ActionResult Pending()
        {
            using (context)
            {
                List<TripsTable> model = context.TripsTables.Where(o => o.TripStatus == (int)ETripStatus.PENDING).ToList();

                return View(model);
            }
        }

        public ActionResult All()
        {
            using (context)
            {
                List<TripsTable> model = context.TripsTables.ToList();

                return View(model);
            }
        }

        public ActionResult Completed()
        {
            using (context)
            {
                List<TripsTable> model = context.TripsTables.Where(o => o.TripStatus == (int)ETripStatus.COMPLETED).ToList();

                return View(model);
            }
        }

        public ActionResult Approved()
        {
            using (context)
            {
                List<TripsTable> model = context.TripsTables.Where(o => o.TripStatus == (int)ETripStatus.APPROVED).ToList();

                return View(model);
            }
        }

        public ActionResult Rejected()
        {
            using (context)
            {
                List<TripsTable> model = context.TripsTables.Where(o => o.TripStatus == (int)ETripStatus.REJECTED).ToList();

                return View(model);
            }
        }

        public ActionResult Cancelled()
        {
            using (context)
            {
                List<TripsTable> model = context.TripsTables.Where(o => o.TripStatus == (int)ETripStatus.CANCELLED).ToList();

                return View(model);
            }
        }

        public ActionResult ViewTrip(int id)
        {
            using (context)
            {
                TripViewModelAdmin model = new TripViewModelAdmin();
                TripsTable trip = context.TripsTables.Where(x => x.TripID == id).FirstOrDefault();
                RiderDetailsTable rider = context.RiderDetailsTables.Where(x => x.RiderID == trip.UserID).FirstOrDefault();
                TripDetaisTable tripDetail = context.TripDetaisTables.Where(x => x.TripID == trip.TripDetailsTableID).FirstOrDefault();
                if (trip.DriverID != null)
                {
                    DriverDetailsTable driver = context.DriverDetailsTables.Where(x => x.DriverID == trip.DriverID).FirstOrDefault();
                    model.Driver = driver;
                }
                model.Trips = trip;
                model.Rider = rider;
                model.TripDetails = tripDetail;

                return PartialView("ViewTripPV", model);
            }
        }

        public ActionResult ApproveTrip(int id)
        {
            using (context)
            {
                TripsTable trip = context.TripsTables.Where(x => x.TripID == id).FirstOrDefault();
                List<DriverDetailsTable> model = context.DriverDetailsTables.Where(x => x.VehicleType == trip.VehicleType).ToList();
                return PartialView("ApprovePV", model);
            }
        }

        public ActionResult LoadDrivers(int tripId, int vehicleType)
        {
            List<SelectListItem> drivers = context.DriverDetailsTables.Where(x => x.VehicleType == vehicleType).Select(c => new SelectListItem { Text = c.DriverCode, Value = c.DriverID.ToString() }).ToList();
            ViewBag.DriverList = drivers;
            var model = new ApproveTripModel
            {
                TripID = tripId
            };
            return PartialView("ApprovePV", model);
        }

        [HttpPost]
        public ActionResult ApproveTrip(ApproveTripModel model)
        {
            using (context)
            {
                TripsTable trip = context.TripsTables.Where(x => x.TripID == model.TripID).FirstOrDefault();
                trip.DriverID = model.DriverID;
                trip.TripStatus = (int)ETripStatus.APPROVED;
                context.Entry(trip).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Approved");
            }
        }

        public ActionResult RejectTrip(int tripId)
        {
            using (context)
            {
                TripsTable trip = context.TripsTables.Where(x => x.TripID == tripId).FirstOrDefault();
                trip.TripStatus = (int)ETripStatus.REJECTED;

                context.Entry(trip).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }

    }
}