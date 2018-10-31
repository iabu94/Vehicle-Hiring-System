using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TaxiService.Models;

namespace TaxiService.Controllers
{
    public class ValidateLoginController : Controller
    {
        TaxiServiceEntities context = new TaxiServiceEntities();
        // GET: ValidateLogin
        [HttpPost]
        public ActionResult Authorize(UserLoginViewModel model)
        {
            using (context)
            {
                UserLoginDetail obj = context.UserLoginDetails.Where(x => x.UserLoginEmail == model.UserName || x.UserLoginMobile.ToString() == model.UserName).FirstOrDefault();
                if (obj == null)
                {
                    TempData["ValidateMessage"] = "The username is doesnt exist in the context";
                    return RedirectToAction("Login", "UserHome", model);
                }
                else
                {
                    if (obj.UserLoginPassword.Contains("$"))
                    {
                        if (BCrypt.Net.BCrypt.Verify(model.Password, obj.UserLoginPassword))
                        {
                            if (obj.UserType == 2)
                            {
                                Session["LoggedUserID"] = obj.UserTableID;
                                DriverDetailsTable dr = context.DriverDetailsTables.Where(x => x.DriverID == obj.UserTableID).FirstOrDefault();
                                Session["LoggedUserName"] = dr.FirstName;
                                Session["ImageUrl"] = dr.UserImageUrl;

                                return RedirectToAction("Home", "DriverDash", new { area = "DriverHome" });
                            }
                            else if (obj.UserType == 3)
                            {
                                Session["LoggedUserID"] = obj.UserTableID;
                                RiderDetailsTable rd = context.RiderDetailsTables.Where(x => x.RiderID == obj.UserTableID).FirstOrDefault();
                                Session["LoggedUserName"] = rd.FirstName;
                                return RedirectToAction("Index", "UserHome", new { area = "" });
                            }
                            else if (obj.UserType == 1)
                            {
                                Session["LoggedUserID"] = obj.UserTableID;
                                AdminDetailsTable ad = context.AdminDetailsTables.Where(x => x.AdminID == obj.UserTableID).FirstOrDefault();
                                Session["LoggedUserName"] = ad.FirstName;
                                Session["ImageUrl"] = ad.UserImageUrl;
                                return RedirectToAction("Index", "Home", new { area = "Admin" });
                            }
                        }

                    }
                    else if (obj.UserLoginPassword == model.Password)
                    {
                        if (obj.UserType == 2)
                        {
                            Session["LoggedUserID"] = obj.UserTableID;
                            DriverDetailsTable dr = context.DriverDetailsTables.Where(x => x.DriverID == obj.UserTableID).FirstOrDefault();
                            Session["LoggedUserName"] = dr.FirstName;
                            Session["ImageUrl"] = dr.UserImageUrl;

                            return RedirectToAction("Home", "DriverDash", new { area = "DriverHome" });
                        }
                        else if (obj.UserType == 3)
                        {
                            Session["LoggedUserID"] = obj.UserTableID;
                            RiderDetailsTable rd = context.RiderDetailsTables.Where(x => x.RiderID == obj.UserTableID).FirstOrDefault();
                            Session["LoggedUserName"] = rd.FirstName;
                            return RedirectToAction("Index", "UserHome", new { area = "" });
                        }
                        else if (obj.UserType == 1)
                        {
                            Session["LoggedUserID"] = obj.UserTableID;
                            AdminDetailsTable ad = context.AdminDetailsTables.Where(x => x.AdminID == obj.UserTableID).FirstOrDefault();
                            Session["LoggedUserName"] = ad.FirstName;
                            Session["ImageUrl"] = ad.UserImageUrl;
                            return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                    }
                    else
                    {
                        TempData["ValidateMessage"] = "The password is incorrect";
                        ViewBag.Message = "The password is incorrect";
                        return RedirectToAction("Login", "UserHome", model);
                    }
                }
            }
            return RedirectToAction("Login", "UserHome", model);
        }

        public JsonResult Authorize(ViewTripModel model)
        {
            using (context)
            {
                UserLoginDetail obj = context.UserLoginDetails.Where(x => x.UserLoginEmail == model.UserName || x.UserLoginMobile.ToString() == model.UserName).FirstOrDefault();
                if (obj == null)
                {
                    TempData["ValidateMessage"] = "The username is doesnt exist in the context";
                    return Json(Response, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (model.Password.Contains("$"))
                    {
                        if (BCrypt.Net.BCrypt.Verify(model.Password, obj.UserLoginPassword))
                        {
                            if (obj.UserType == 2)
                            {
                                Session["LoggedUserID"] = obj.UserTableID;
                                DriverDetailsTable dr = context.DriverDetailsTables.Where(x => x.DriverID == obj.UserTableID).FirstOrDefault();
                                Session["LoggedUserName"] = dr.FirstName;
                                Session["ImageUrl"] = dr.UserImageUrl;

                               // return RedirectToAction("Home", "DriverDash", new { area = "DriverHome" });
                            }
                            else if (obj.UserType == 3)
                            {
                                Session["LoggedUserID"] = obj.UserTableID;
                                RiderDetailsTable rd = context.RiderDetailsTables.Where(x => x.RiderID == obj.UserTableID).FirstOrDefault();
                                Session["LoggedUserName"] = rd.FirstName;
                               // return RedirectToAction("Index", "UserHome", new { area = "" });
                            }
                            else if (obj.UserType == 1)
                            {
                                Session["LoggedUserID"] = obj.UserTableID;
                                AdminDetailsTable ad = context.AdminDetailsTables.Where(x => x.AdminID == obj.UserTableID).FirstOrDefault();
                                Session["LoggedUserName"] = ad.FirstName;
                                Session["ImageUrl"] = ad.UserImageUrl;
                               // return RedirectToAction("Index", "Home", new { area = "Admin" });
                            }
                        }

                    }
                    else if (obj.UserLoginPassword == model.Password)
                    {
                        if (obj.UserType == 2)
                        {
                            Session["LoggedUserID"] = obj.UserTableID;
                            DriverDetailsTable dr = context.DriverDetailsTables.Where(x => x.DriverID == obj.UserTableID).FirstOrDefault();
                            Session["LoggedUserName"] = dr.FirstName;
                            Session["ImageUrl"] = dr.UserImageUrl;

                            //return RedirectToAction("Home", "DriverDash", new { area = "DriverHome" });
                        }
                        else if (obj.UserType == 3)
                        {
                            Session["LoggedUserID"] = obj.UserTableID;
                            RiderDetailsTable rd = context.RiderDetailsTables.Where(x => x.RiderID == obj.UserTableID).FirstOrDefault();
                            Session["LoggedUserName"] = rd.FirstName;
                           // return RedirectToAction("Index", "UserHome", new { area = "" });
                        }
                        else if (obj.UserType == 1)
                        {
                            Session["LoggedUserID"] = obj.UserTableID;
                            AdminDetailsTable ad = context.AdminDetailsTables.Where(x => x.AdminID == obj.UserTableID).FirstOrDefault();
                            Session["LoggedUserName"] = ad.FirstName;
                            Session["ImageUrl"] = ad.UserImageUrl;
                            //return RedirectToAction("Index", "Home", new { area = "Admin" });
                        }
                    }
                    else
                    {
                        TempData["ValidateMessage"] = "The password is incorrect";
                        ViewBag.Message = "The password is incorrect";
                        //return RedirectToAction("Login", "UserHome", model);
                    }
                }
            }
            return Json(Response);
        }
    }
}