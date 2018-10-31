using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiService.Models;
using static TaxiService.Common.Enum;

namespace TaxiService.Controllers
{
    public class TripsController : Controller
    {
        private TaxiServiceEntities context = new TaxiServiceEntities();

        public ActionResult Index()
        {
            List<TripDetaisTable> model = context.TripDetaisTables.ToList();
            return View(model);
        }

        public ActionResult ViewTrip(int tripId)
        {
            ViewTripModel trips = new ViewTripModel();

            TripDetaisTable trip = context.TripDetaisTables.Where(x => x.TripID == tripId).SingleOrDefault();

            trips.tripId = trip.TripID;
            trips.CarAmount = trip.CarPerDay;
            trips.VanAmount = trip.VanPerDay;
            trips.BusAmount = trip.BusPerDay;

            return View(trips);
        }

        public JsonResult CalAmount(int days, int tripId)
        {
            ViewTripModel trips = new ViewTripModel();
            TripDetaisTable trip = context.TripDetaisTables.Where(x => x.TripID == tripId).SingleOrDefault();

            trips.tripId = trip.TripID;
            switch (days)
            {
                case 1:
                    trips.CarAmount = trip.CarPerDay;
                    trips.VanAmount = trip.VanPerDay;
                    trips.BusAmount = trip.BusPerDay;
                    break;
                case 2:
                    trips.CarAmount = (trip.CarPerDay * days) - 500;
                    trips.VanAmount = (trip.VanPerDay * days) - 1000;
                    trips.BusAmount = (trip.BusPerDay * days) - 2000;
                    break;
                case 3:
                    trips.CarAmount = trip.CarPerDay * days - 1000;
                    trips.VanAmount = trip.VanPerDay * days - 2000;
                    trips.BusAmount = trip.BusPerDay * days - 3000;
                    break;
                case 4:
                    trips.CarAmount = trip.CarPerDay * days - 1500;
                    trips.VanAmount = trip.VanPerDay * days - 3000;
                    trips.BusAmount = trip.BusPerDay * days - 4000;
                    break;
                case 5:
                    trips.CarAmount = trip.CarPerDay * days - 2000;
                    trips.VanAmount = trip.VanPerDay * days - 4000;
                    trips.BusAmount = trip.BusPerDay * days - 5000;
                    break;
            }

            return Json(trips, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SubmitTrip(ViewTripModel model)
        {
            try
            {
                if (Session["LoggedUserID"] == null)
                {
                    return View("Login", model);
                }
                else if (Session["LoggedUserID"].ToString() == EUserType.ADMIN.ToString())
                {
                    TempData["ValidateMessage"] = "You Can't apply for the trip with an ADMIN account. Try with a RIDER account.";
                    return View("Login", model);
                }
                else if (Session["LoggedUserID"].ToString() == EUserType.DRIVER.ToString())
                {
                    TempData["ValidateMessage"] = "You Can't apply for the trip with an DRIVER account. Try with a RIDER account.";
                    return View("Login", model);
                }
                else
                {
                    TripsTable trip = new TripsTable();
                    trip.UserID = Int32.Parse(Session["LoggedUserID"].ToString());
                    trip.TripDetailsTableID = model.tripId;
                    trip.TripStatus = (int)ETripStatus.PENDING;
                    trip.PickupDate = model.Date;
                    trip.VehicleType = model.VehicleType;
                    trip.TripDays = model.TripDays;
                    switch (model.VehicleType)
                    {
                        case 2:
                            trip.Amount = model.CarAmount.ToString();
                            break;
                        case 3:
                            trip.Amount = model.VanAmount.ToString();
                            break;
                        case 4:
                            trip.Amount = model.BusAmount.ToString();
                            break;
                        default:
                            trip.Amount = model.CarAmount.ToString();
                            break;
                    }

                    if (model.Time.Substring(model.Time.Length - 2) == "AM")
                    {
                        trip.PickupTime = TimeSpan.Parse(model.Time.Remove(model.Time.Length - 3));
                    }
                    else if (model.Time.Substring(model.Time.Length - 2) == "PM")
                    {
                        model.Time = model.Time.Remove(model.Time.Length - 3);
                        int hr = Int32.Parse(model.Time.Remove(model.Time.IndexOf(":"))) + 12;
                        if (hr == 24)
                        {
                            hr = 12;
                        }
                        string min = model.Time.Substring(model.Time.IndexOf(":") + 1);
                        trip.PickupTime = TimeSpan.Parse(hr.ToString() + ":" + min);
                    }

                    context.TripsTables.Add(trip);
                    context.SaveChanges();

                    return View();
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Index");
            }


        }

        public ActionResult ValidateTripLogin(ViewTripModel model)
        {
            using (context)
            {
                UserLoginDetail obj = context.UserLoginDetails.Where(x => x.UserLoginEmail == model.UserName || x.UserLoginMobile.ToString() == model.UserName).FirstOrDefault();
                if (obj == null)
                {
                    TempData["ValidateMessage"] = "The username is doesnt exist in the context";
                    return RedirectToAction("SubmitTrip", "Trips", model);
                }
                else
                {
                    if (obj.UserLoginPassword.Contains("$"))
                    {
                        if (BCrypt.Net.BCrypt.Verify(model.Password, obj.UserLoginPassword))
                        {
                            if (obj.UserType == 2)
                            {
                                TempData["ValidateMessage"] = "You Cannot Request With Driver Account";

                                return RedirectToAction("SubmitTrip", "Trips", model);
                            }
                            else if (obj.UserType == 3)
                            {
                                Session["LoggedUserID"] = obj.UserTableID;
                                RiderDetailsTable rd = context.RiderDetailsTables.Where(x => x.RiderID == obj.UserTableID).FirstOrDefault();
                                Session["LoggedUserName"] = rd.FirstName;

                                return RedirectToAction("SubmitTrip", "Trips", model);
                            }
                            else if (obj.UserType == 1)
                            {
                                TempData["ValidateMessage"] = "You Cannot Request With Admin Account";

                                return RedirectToAction("SubmitTrip", "Trips", model);
                            }
                        }

                    }
                    else if (obj.UserLoginPassword == model.Password)
                    {
                        if (obj.UserType == 2)
                        {
                            TempData["ValidateMessage"] = "You Cannot Request With Driver Account";

                            return RedirectToAction("SubmitTrip", "Trips", model);
                        }
                        else if (obj.UserType == 3)
                        {
                            Session["LoggedUserID"] = obj.UserTableID;
                            RiderDetailsTable rd = context.RiderDetailsTables.Where(x => x.RiderID == obj.UserTableID).FirstOrDefault();
                            Session["LoggedUserName"] = rd.FirstName;

                            return RedirectToAction("SubmitTrip", "Trips", model);
                        }
                        else if (obj.UserType == 1)
                        {
                            TempData["ValidateMessage"] = "You Cannot Request With Admin Account";

                            return RedirectToAction("SubmitTrip", "Trips", model);
                        }
                    }
                    else
                    {
                        TempData["ValidateMessage"] = "The password is incorrect";
                        ViewBag.Message = "The password is incorrect";
                        return RedirectToAction("SubmitTrip", "Trips", model);
                    }
                }
            }
            return RedirectToAction("SubmitTrip", "Trips", model);
        }
    }
}