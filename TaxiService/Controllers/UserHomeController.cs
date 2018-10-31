using GMap.NET;
using GMap.NET.MapProviders;
using GoogleMaps.LocationServices;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using TaxiService.Models;
using System.Device.Location;
using GMap.NET.WindowsForms.Markers;

namespace TaxiService.Controllers
{
    public class UserHomeController : Controller
    {
        TaxiServiceEntities context = new TaxiServiceEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult NewBooking(HomeBookingModel model)
        {
            model.PickUpTime = DateTime.Now;

            Tuple<string, string> result = CalculateDistanceDuration(model.LocationFrom, model.LocationTo);
            model.DistanceKm = float.Parse(result.Item1) / 1000;

            return View(model);
        }

        [HttpPost]
        public ActionResult BookingDetails(HomeBookingModel model)
        {
            using (TaxiServiceEntities context = new TaxiServiceEntities())
            {
                Tuple<string, string> result = CalculateDistanceDuration(model.LocationFrom, model.LocationTo);
                model.DistanceKm = float.Parse(result.Item1) / 1000;
                model.DurationMin = Int32.Parse(result.Item2);

                #region getVehicleAndLocation

                //List<Vehicle> getVehicleAndLocation = context.Vehicles.Where(x => x.IsAvailable == "YES" && x.VehicleTypeID == model.VehicleType).ToList();

                //if (getVehicleAndLocation == null)
                //{
                //    ViewBag.MessageResponse = "No Vehicles available";
                //    return View();
                //}

                //foreach (var item in getVehicleAndLocation)
                //{
                //    Tuple<string, string> res = CalculateDistanceDuration(model.LocationFrom, item.CurrentLocation);
                //    if (float.Parse(res.Item1) < 50000)
                //    {
                //        ViewBag.MessageResponse = "Confirmed the vehicle";
                //        model.VehicleID = item.VehicleID;
                //        break;
                //    }
                //    else
                //    {
                //        ViewBag.MessageResponse = "No Vehicles available";
                //        continue;
                //    }
                //}
                #endregion

                if (model.VehicleID == 0)
                {
                    return View();
                }

                #region Hire Fares
                if ((model.DistanceKm <= 1 && (model.VehicleType == 1 || model.VehicleType == 2)) || (model.DistanceKm <= 8 && model.VehicleType == 3))
                {
                    switch (model.VehicleType)
                    {
                        case 1:
                            model.HirePrice = 150;
                            break;
                        case 2:
                            model.HirePrice = 200;
                            break;
                        case 3:
                            model.HirePrice = 1000;
                            break;
                    }
                }
                else
                {
                    switch (model.VehicleType)
                    {
                        case 1:
                            model.HirePrice = (model.DistanceKm - 1) * 50 + 150;
                            break;
                        case 2:
                            model.HirePrice = (model.DistanceKm - 1) * 50 + 200;
                            break;
                        case 3:
                            model.HirePrice = (model.DistanceKm - 8) * 50 + 1000;
                            break;
                    }
                }
                #endregion  `

                #region Assign Model values

                //UserDetail user = context.UserDetails.Where(o => o.UserLoginID == Int32.Parse(Session["LoginUserID"].ToString())).FirstOrDefault();

                //HireDetail hireDetail = new HireDetail();
                //hireDetail.PickupLocation = model.LocationFrom;
                //hireDetail.DropLocation = model.LocationTo;
                //hireDetail.DistanceKm = model.DistanceKm;
                //hireDetail.PickupDateTime = model.PickUpTime;
                //hireDetail.EstimatedFare = model.HirePrice;
                //hireDetail.UserID = user.UserID;
                //hireDetail.VehicleID = model.VehicleID;
                //hireDetail.VehicleTypeID = model.VehicleType;
                //hireDetail.HireStatusID = 1;

                //context.HireDetails.Add(hireDetail);
                //context.SaveChanges();

                ConfirmedDetailViewModel viewModel = new ConfirmedDetailViewModel();
                //viewModel.PickupLocation = hireDetail.PickupLocation;
                //viewModel.PickupDateTime = hireDetail.PickupDateTime;
                //viewModel.DropLocation = hireDetail.DropLocation;
                //viewModel.EstimatedFare = hireDetail.EstimatedFare;
                //viewModel.VehicleNumber = hireDetail.Vehicle.VehicleNumber;
                //viewModel.VehicleBrand = hireDetail.Vehicle.VehicleBrand;
                viewModel.ContactNo = 12346666;
                //viewModel.Image = hireDetail.Vehicle.Driver.DriverCode;

                #endregion

                return View("ConfirmedDetails", viewModel);
            }
        }

        public ActionResult ConfirmedDetails(ConfirmedDetailViewModel model)
        {
            return View(model);
        }

        public ActionResult Login(UserLoginViewModel model)
        {
            return View(model);
        }

        public Tuple<string, string> CalculateDistanceDuration(string origin, string destination)
        {
            try
            {
                #region Using MSSQL Database Distance Finder
                string org = origin;
                string des = destination;

                //region for the origin search
                #region Origin Search
                LocationsTable objLocationOrigin = new LocationsTable();
                if (org.Contains(" "))
                {
                    for (int i = 0; i < origin.Split().Length; i++)
                    {
                        //check whether the entered loacation available in the database
                        if (context.LocationsTables.Any(o => o.City == org))
                        {
                            objLocationOrigin = context.LocationsTables.Where(x => x.City == org).FirstOrDefault();
                            break;
                        }
                        else
                        {
                            org = org.Substring(0, org.LastIndexOf(" ") + 1);
                            org = org.Trim();
                            if (org.EndsWith(","))
                            {
                                org = org.Substring(0, org.Length - 1);
                            }
                        }
                    }
                }
                else
                {
                    if (context.LocationsTables.Any(o => o.City == org))
                    {
                        objLocationOrigin = context.LocationsTables.Where(x => x.City == org).FirstOrDefault();
                    }
                }
                #endregion

                //region for the destination search
                #region Destination search
                LocationsTable objLocationDestination = new LocationsTable();
                if (des.Contains(" "))
                {
                    for (int i = 0; i < destination.Split().Length; i++)
                    {
                        //check whether the entered loacation available in the database
                        if (context.LocationsTables.Any(o => o.City == des))
                        {
                            objLocationDestination = context.LocationsTables.Where(x => x.City == des).FirstOrDefault();
                            break;
                        }
                        else
                        {
                            des = des.Substring(0, des.LastIndexOf(" ") + 1);
                            des = des.Trim();
                            if (des.EndsWith(","))
                            {
                                des = des.Substring(0, des.Length - 1);
                            }
                        }
                    }
                }
                else
                {
                    if (context.LocationsTables.Any(o => o.City == des))
                    {
                        objLocationDestination = context.LocationsTables.Where(x => x.City == des).FirstOrDefault();
                    }
                }
                #endregion

                //Calculate the distance
                if (objLocationOrigin.LocationID != 0 && objLocationDestination.LocationID != 0)
                {
                    var oCoord = new GeoCoordinate(Convert.ToDouble(objLocationOrigin.Latitude), Convert.ToDouble(objLocationOrigin.Longtitude));
                    var dCoord = new GeoCoordinate(Convert.ToDouble(objLocationDestination.Latitude), Convert.ToDouble(objLocationDestination.Longtitude));

                    return new Tuple<string, string>(oCoord.GetDistanceTo(dCoord).ToString(), "4");
                }

                #endregion
                
                else if (origin != null && destination != null)
                {
                    #region Using Google Maps API

                    string url = "https://maps.googleapis.com/maps/api/distancematrix/xml?origins=" + origin + "&destinations=" + destination + "&mode=driving&sensor=false&language=en-EN&units=metric&key=AIzaSyDdsaERISYyGWXAR5s8aBaumYo4gGrOxh0";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                    WebResponse response = request.GetResponse();
                    Stream dataStream = response.GetResponseStream();
                    StreamReader sreader = new StreamReader(dataStream);
                    string responsereader = sreader.ReadToEnd();
                    response.Close();
                    XmlDocument xmldoc = new XmlDocument();
                    xmldoc.LoadXml(responsereader);
                    string distanceMeter = "0";
                    string durationMin = "0";
                    if (xmldoc.GetElementsByTagName("status")[0].ChildNodes[0].InnerText == "OK")
                    {
                        XmlNodeList distance = xmldoc.GetElementsByTagName("distance");
                        distanceMeter = distance[0].ChildNodes[0].InnerText.ToString();

                        XmlNodeList duration = xmldoc.GetElementsByTagName("duration");
                        durationMin = duration[0].ChildNodes[0].InnerText.ToString();

                        Tuple<string, string> t = new Tuple<string, string>(distanceMeter, durationMin);
                        return t;
                    }
                    else
                    {
                        return new Tuple<string, string>("1000", "4");
                    }
                    #endregion
                }

                else
                {
                    Tuple<string, string> t = new Tuple<string, string>("1000", "4");
                    return t;
                }
            }
            catch (Exception e)
            {
                Tuple<string, string> t = new Tuple<string, string>("1000", "4");
                return t;
            }
        }

        public ActionResult RegisterRider()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterRider(RiderDetailsTable model)
        {
            try
            {
                using (context)
                {
                    model.Password = BCrypt.Net.BCrypt.HashString(model.Password);

                    model.RegisteredDate = DateTime.Now;
                    model.Status = 1;
                    model.CurrentLocation = "Kiribathgoda, Sri Lanka";
                    context.RiderDetailsTables.Add(model);
                    context.SaveChanges();
                }
                return RedirectToAction("Login", "UserHome", new { area = "" });
            }
            catch (DbEntityValidationException e)
            {
                List<string> errorLog = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    errorLog.Add(eve.ValidationErrors.ToString());
                }
                throw;
            }
        }

        public JsonResult CalAmount(string from, string to)
        {
            Tuple<string, string> result = CalculateDistanceDuration(from, to);

            string distance = result.Item1;

            string NanoCarBill = "150";
            string CarBill = "200";
            string VanBill = "1000";
            
            int index = distance.IndexOf(".");
            if (index > 0)
            {
                distance = distance.Substring(0, index);
            }

            if ((Int32.Parse(distance) / 1000) > 1)
            {
                NanoCarBill = ((((Int32.Parse(distance) / 1000) - 1) * 50) + 150).ToString();
                CarBill = ((((Int32.Parse(distance) / 1000) - 1) * 50) + 200).ToString();
            }
            if ((Int32.Parse(distance) / 1000) > 8)
            {
                VanBill = ((((Int32.Parse(distance) / 1000) - 8) * 50) + 1000).ToString();
            }

            //pass multiple variable
            var res = new { NanoCar = NanoCarBill, Car = CarBill, Van = VanBill };
            return Json(res, JsonRequestBehavior.AllowGet);
        }
    }
}