using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiService.Models;

namespace TaxiService.Controllers
{
    public class SellerController : Controller
    {
        private TaxiServiceEntities context = new TaxiServiceEntities();

        public ActionResult Index()
        {
            ViewAddsModel model = new ViewAddsModel();

            List<SellingVehicleDetail> VehicleModel = context.SellingVehicleDetails.OrderByDescending(x=>x.UploadedDate).ToList();
            List<SellingVehicleImageTable> ImagesModel = new List<SellingVehicleImageTable>();

            foreach (var item in VehicleModel)
            {
                ImagesModel.Add(context.SellingVehicleImageTables.Where(x => x.SellingID == item.SellingID).FirstOrDefault());
            }

            model.VehicleDetails = VehicleModel;
            model.VehicleImages = ImagesModel;

            return View(model);
        }
    }
}