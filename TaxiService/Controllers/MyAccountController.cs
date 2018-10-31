using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiService.Common;

namespace TaxiService.Controllers
{
    public class MyAccountController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}