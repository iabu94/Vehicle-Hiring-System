using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiService.Models;

namespace TaxiService.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        // GET: Admin/Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Authorize(UserLoginDetail user)
        {
            using (TaxiServiceEntities dbEntities = new TaxiServiceEntities())
            {
                var userDetails = dbEntities.UserLoginDetails.Where(x => x.UserLoginEmail == user.UserLoginEmail && x.UserLoginPassword == user.UserLoginPassword).FirstOrDefault();
                if (userDetails == null)
                {
                    ViewBag.ErrorMessage = "Username or password is incorrect";
                    Session["LoginUserID"] = null;
                    //user.ErrorMessage = "Username or password is incorrect";
                    return View("Index", user);
                }
                else
                {
                    Session["LoginUserID"] = userDetails.UserLoginID;
                    Session["LoginUserName"] = userDetails.UserLoginMobile;
                    return RedirectToAction("Index", "Home");
                }
            }
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login", "UserHome",new { area = ""});
        }
    }
}