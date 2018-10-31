using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiService.Common;
using TaxiService.Models;
using static TaxiService.Common.Enum;
using Nexmo.Api;

namespace TaxiService.Areas.Admin.Controllers
{
    [SessionExpire]
    public class DriverController : Controller
    {
        private TaxiServiceEntities context = new TaxiServiceEntities();

        public ActionResult Active()
        {
            using (context)
            {
                List<DriverDetailsTable> driverList = context.DriverDetailsTables.Where(x=>x.Status==(int)EUserStatus.ACTIVE).ToList();
                return View(driverList);
            }
        }

        public ActionResult Pending()
        {
            using (context)
            {
                List<DriverDetailsTable> driverList = context.DriverDetailsTables.Where(x=>x.Status==(int)EUserStatus.PENDING).ToList();
                return View(driverList);
            }
        }

        public ActionResult Rejected()
        {
            using (context)
            {
                List<DriverDetailsTable> driverList = context.DriverDetailsTables.Where(x => x.Status == (int)EUserStatus.REJECTED).ToList();
                return View(driverList);
            }
        }

        public ActionResult AddDriver()
        {
            return PartialView("AddDriverPV");
        }

        [HttpPost]
        public ActionResult AddDriver(DriverDetailsTable model)
        {
            using (context)
            {
                model.IsDeleted = (int)EIsDeleted.NO;
                model.IsOnline = (int)EAvailability.OFFLINE;
                model.RegisteredDate = DateTime.Now;
                model.Status = (int)EUserStatus.PENDING;

                HttpPostedFileBase file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    string fileName = model.FirstName + DateTime.Now.ToString("yyMMddhhmmss")+Path.GetExtension(file.FileName);

                    model.UserImageUrl = "~/Images/Drivers/" + fileName;

                    fileName = Path.Combine(
                        Server.MapPath("~/Images/Drivers/"), fileName);
                    file.SaveAs(fileName);
                }

                context.DriverDetailsTables.Add(model);
                context.SaveChanges();

                TempData["Success"] = "Added Successfully!";

                return RedirectToAction("Active");
            }
        }

        public ActionResult EditDriver(int driverId)
        {
            using (context)
            {
                DriverDetailsTable rdr = context.DriverDetailsTables.Where(x => x.DriverID == driverId).FirstOrDefault();
                return PartialView("EditDriverPV", rdr);
            }
        }

        [HttpPost]
        public ActionResult EditDriver(DriverDetailsTable driver)
        {
            using (context)
            {
                context.Entry(driver).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Active", "Driver", new { area = "Admin" });
            }
        }

        public ActionResult DeleteDriver(int driverId)
        {
            using (context)
            {
                DriverDetailsTable dr = context.DriverDetailsTables.Where(x => x.DriverID == driverId).FirstOrDefault();
                dr.IsDeleted = (int)EIsDeleted.YES;

                context.Entry(dr).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult ViewDriver(int driverId)
        {
            
            using (context)
            {
                DriverDetailsTable driver = context.DriverDetailsTables.Where(x => x.DriverID == driverId).FirstOrDefault();
                return PartialView("ViewDriverPV", driver);
            }
        }

        public ActionResult ApproveDriver(int driverId)
        {
            using (context)
            {
                DriverDetailsTable dr = context.DriverDetailsTables.Where(x => x.DriverID == driverId).FirstOrDefault();
                dr.Status = (int)EUserStatus.ACTIVE;

                context.Entry(dr).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RejectDriver(int driverId)
        {
            using (context)
            {
                DriverDetailsTable dr = context.DriverDetailsTables.Where(x => x.DriverID == driverId).FirstOrDefault();
                dr.Status = (int)EUserStatus.REJECTED;

                context.Entry(dr).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RestoreDriver(int driverId)
        {
            using (context)
            {
                DriverDetailsTable dr = context.DriverDetailsTables.Where(x => x.DriverID == driverId).FirstOrDefault();
                dr.Status = (int)EUserStatus.PENDING;

                context.Entry(dr).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Management()
        {
            using (TaxiServiceEntities db = new TaxiServiceEntities())
            {
                return View();
            }
        }
    }
}