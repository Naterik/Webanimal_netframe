using NetFramwork_WildNature.Areas.Admin.Models;
using NetFramwork_WildNature.Common;
using NetFramwork_WildNature.Dao;
using NetFramwork_WildNature.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NetFramwork_WildNature.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private WildNature db = new WildNature();

        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string email, string pass)
        {
            var acc = db.Accounts.SingleOrDefault(m => m.Email.Trim().ToLower() == email.Trim().ToLower() && m.Password == pass);

            if (acc != null)
            {
                Session["user"] = acc;
                FormsAuthentication.SetAuthCookie(acc.Email, false);

                if (acc.RoleID == 1)
                {
                    return RedirectToAction("Index");
                }
                else if (acc.RoleID == 2)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else if (acc.RoleID == 3)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }

            TempData["error"] = "Invalid account, please try again.";
            return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("user");
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }
}