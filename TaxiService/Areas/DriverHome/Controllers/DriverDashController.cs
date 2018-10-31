using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxiService.Areas.DriverHome.Controllers
{
    public class DriverDashController : Controller
    {
        // GET: DriverHome/DriverDash
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult Pending()
        {
            return View();
        }

        public ActionResult Accepted()
        {
            return View();
        }

        public ActionResult Completed()
        {
            return View();
        }

        public ActionResult Cancelled()
        {
            return View();
        }

        public ActionResult VehicleInfo()
        {
            return View();
        }

        public ActionResult Payment()
        {
            return View();
        }

        public ActionResult ProfileInfo()
        {
            return View();
        }

        public ActionResult Logout()
        {
            return RedirectToAction("Index","Login", new { area="Admin"});
        }
    }
}