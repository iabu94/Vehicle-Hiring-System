using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiService.Common;
using TaxiService.Models;

namespace TaxiService.Areas.Admin.Controllers
{
    [SessionExpire]
    public class SellerAdminController : Controller
    {
        private TaxiServiceEntities context = new TaxiServiceEntities();

        public ActionResult Index()
        {
            List<SellingVehicleDetail> model = context.SellingVehicleDetails.OrderByDescending(x => x.UploadedDate).ToList();
            return View(model);
        }

        public ActionResult AddSellingVehicle()
        {
            return PartialView("AddSellingVehiclePV");
        }

        [HttpPost]
        public ActionResult AddSellingVehicle(SellingVehicleDetail model, IEnumerable<HttpPostedFileBase> files)
        {
            model.UploadedDate = DateTime.Now;
            model.VehicleDescription = model.VehicleDescription.Replace("\r\n", "<br />");
            context.SellingVehicleDetails.Add(model);
            context.SaveChanges();

            List<SellingVehicleImageTable> images = new List<SellingVehicleImageTable>();

            int i = 1;
            foreach (var file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    SellingVehicleImageTable img = new SellingVehicleImageTable();
                    string fileName = i + DateTime.Now.ToString("yyMMddhhmmss") + Path.GetExtension(file.FileName);
                    img.ImageUrl = "~/Images/Selling/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Images/Selling/"), fileName);
                    file.SaveAs(fileName);
                    img.SellingID = model.SellingID;
                    images.Add(img);
                    i++;
                }
            }

            foreach (var image in images)
            {
                context.SellingVehicleImageTables.Add(image);
            }

            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult ViewAddvertise(int id)
        {
            SellingVehicleDetail model = context.SellingVehicleDetails.Where(x => x.SellingID == id).FirstOrDefault();
            ViewBag.VehicleImages = context.SellingVehicleImageTables.Where(x => x.SellingID == model.SellingID).ToList();
            return PartialView("ViewSellingVehiclePV", model);
        }

        public ActionResult ViewVehicleImages(int id)
        {
            List<SellingVehicleImageTable> model = context.SellingVehicleImageTables.Where(x => x.SellingID == id).ToList();
            return PartialView("ViewVehicleImagesPV", model);
        }

        public ActionResult DeleteAddvertise(int id)
        {
            SellingVehicleDetail addvrt = context.SellingVehicleDetails.Where(x => x.SellingID == id).FirstOrDefault();
            context.SellingVehicleDetails.Remove(addvrt);

            List<SellingVehicleImageTable> imgs = context.SellingVehicleImageTables.Where(x => x.SellingID == id).ToList();
            foreach (var item in imgs)
            {
                context.SellingVehicleImageTables.Remove(item);
            }
            context.SaveChanges();

            return Json("success", JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditAddvertise(int id)
        {
            SellingVehicleDetail model = context.SellingVehicleDetails.Where(x => x.SellingID == id).FirstOrDefault();
            return PartialView("EditAddvertisePV", model);
        }

        [HttpPost]
        public ActionResult EditAddvertise(SellingVehicleDetail model)
        {
            using (context)
            {
                model.VehicleDescription = model.VehicleDescription.Replace("\r\n", "<br />");
                context.Entry(model).State = System.Data.Entity.EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}