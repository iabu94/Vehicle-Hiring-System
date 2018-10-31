using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiService.Models;

namespace TaxiService.Areas.DriverHome.Controllers
{
    public class LoginController : Controller
    {
        TaxiServiceEntities context = new TaxiServiceEntities();
        // GET: DriverHome/Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RegisterDriver()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterDriver(DriverDetailsTable model)
        {
            try
            {
                HttpPostedFileBase file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    string fileName = model.FirstName + DateTime.Now.ToString("yyMMddhhmmss") + Path.GetExtension(file.FileName);
                    model.UserImageUrl = "~/Images/Drivers/" + fileName;
                    fileName = Path.Combine(
                        Server.MapPath("~/Images/Drivers/"), fileName);
                    file.SaveAs(fileName);
                }
                model.Password = BCrypt.Net.BCrypt.HashString(model.Password);
                model.CurrentLocation = "Maradana, Colombo, Sri Lanka";
                model.RegisteredDate = DateTime.Now;
                model.Status = 1;

                context.DriverDetailsTables.Add(model);
                context.SaveChanges();

                return RedirectToAction("Login", "UserHome", new { area=""});
            }
            catch (Exception e)
            {
                string msg = e.ToString();
                return RedirectToAction("RegisterDriver", "Login", new { area = "DriverHome"});
            }
        }
        
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "UserHome", new { area = "" });
        }
    }
}