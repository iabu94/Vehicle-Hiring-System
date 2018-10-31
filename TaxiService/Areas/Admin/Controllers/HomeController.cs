using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiService.Common;
using TaxiService.Models;

namespace TaxiService.Areas.Admin.Controllers
{
    [SessionExpire]
    public class HomeController : Controller
    {
        private TaxiServiceEntities taxiEntities = new TaxiServiceEntities();

        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }
        
    }
}