using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaxiService.Areas.Admin.Controllers
{
    public class RideController : Controller
    {
        public ActionResult All()
        {
            return View();
        }

        public ActionResult Completed()
        {
            return View();
        }

        public ActionResult Approved()
        {
            return View();
        }

        public ActionResult Rejected()
        {
            return View();
        }

        public ActionResult Cancelled()
        {
            return View();
        }
    }
}