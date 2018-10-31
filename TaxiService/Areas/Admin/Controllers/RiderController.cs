using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiService.Common;
using TaxiService.Models;
using static TaxiService.Common.Enum;

namespace TaxiService.Areas.Admin.Controllers
{
    [SessionExpire]
    public class RiderController : Controller
    {
        TaxiServiceEntities context = new TaxiServiceEntities();

        public ActionResult Active()
        {
            using (context)
            {
                List<RiderDetailsTable> rdr = new List<RiderDetailsTable>();
                rdr = context.RiderDetailsTables.Where(x => x.Status == (int)EUserStatus.ACTIVE && x.IsDeleted != (int)EIsDeleted.YES).ToList();
                return View(rdr);
            }
        }

        public ActionResult Pending()
        {
            using (context)
            {
                List<RiderDetailsTable> rdr = new List<RiderDetailsTable>();
                rdr = context.RiderDetailsTables.Where(x => x.Status == (int)EUserStatus.PENDING && x.IsDeleted != (int)EIsDeleted.YES).ToList();
                return View(rdr);
            }
        }

        public ActionResult Rejected()
        {
            using (context)
            {
                List<RiderDetailsTable> rdr = new List<RiderDetailsTable>();
                rdr = context.RiderDetailsTables.Where(x => x.Status == (int)EUserStatus.REJECTED && x.IsDeleted != (int)EIsDeleted.YES).ToList();
                return View(rdr);
            }
        }

        public ActionResult ViewRider(int riderId)
        {
            using (context)
            {
                RiderDetailsTable rdr = context.RiderDetailsTables.Where(x => x.RiderID == riderId).FirstOrDefault();

                return PartialView("ViewRiderPV", rdr);
            }
        }
        
        public ActionResult AddRider()
        {
            return PartialView("AddRiderPV");
        }

        [HttpPost]
        public ActionResult AddRider(RiderDetailsTable rider)
        {
            using (context)
            {
                rider.IsDeleted = 0;
                rider.RegisteredDate = DateTime.Now;
                rider.Status = (int)EUserStatus.PENDING;

                context.RiderDetailsTables.Add(rider);
                context.SaveChanges();

                return RedirectToAction("Pending", "Rider", new { area = "Admin" });
            }
        }

        public ActionResult EditRider(int riderId)
        {
            using (context)
            {
                RiderDetailsTable rdr = context.RiderDetailsTables.Where(x => x.RiderID == riderId).FirstOrDefault();
                return PartialView("EditRiderPV", rdr);
            }
        }

        [HttpPost]
        public ActionResult EditRider(RiderDetailsTable rider)
        {
            using (context)
            {
                context.Entry(rider).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Active", "Rider", new { area = "Admin" });
            }
        }

        public ActionResult DeleteRider(int riderId)
        {
            using (context)
            {
                RiderDetailsTable rdr = context.RiderDetailsTables.Where(x => x.RiderID == riderId).FirstOrDefault();
                rdr.IsDeleted = (int)EIsDeleted.YES;

                context.Entry(rdr).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult AproveRider(int riderId)
        {
            using (context)
            {
                RiderDetailsTable rdr = context.RiderDetailsTables.Where(x => x.RiderID == riderId).FirstOrDefault();
                rdr.Status = (int)EUserStatus.ACTIVE;

                context.Entry(rdr).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult RejectRider(int riderId)
        {
            using (context)
            {
                RiderDetailsTable rdr = context.RiderDetailsTables.Where(x => x.RiderID == riderId).FirstOrDefault();
                rdr.Status = (int)EUserStatus.REJECTED;

                context.Entry(rdr).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult RestoreRider(int riderId)
        {
            using (context)
            {
                RiderDetailsTable rdr = context.RiderDetailsTables.Where(x => x.RiderID == riderId).FirstOrDefault();
                rdr.Status = (int)EUserStatus.PENDING;

                context.Entry(rdr).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();

                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }
    }
}