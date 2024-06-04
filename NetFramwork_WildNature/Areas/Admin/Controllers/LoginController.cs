using NetFramwork_WildNature.Common;
using NetFramwork_WildNature.Db;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace NetFramwork_WildNature.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private WildNature db = new WildNature();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "ID,Name,Code,Email,Password,CreateDate,RoleID,State")] Account account, string passConfirm)
        {
            if (ModelState.IsValid)
            {
                if (db.Accounts.Any(a => a.Email.Trim().ToLower() == account.Email.Trim().ToLower()))
                {
                    TempData["error"] = "Email đã tồn tại. Vui lòng nhập email khác.";
                    return View(account);
                }
                if (db.Accounts.Any(a => a.Name.Trim().ToLower() == account.Name.Trim().ToLower()))
                {
                    TempData["error"] = "Tên người dùng đã tồn tại. Vui lòng nhập tên người dùng khác.";
                    return View(account);
                }
                if (db.Accounts.Any(a => a.Password.Trim().ToLower() == account.Password.Trim().ToLower()))
                {
                    TempData["error"] = "Mật khẩu đã tồn tại. Vui lòng nhập mật khẩu khác.";
                    return View(account);
                }

                if (account.Password != passConfirm)
                {
                    TempData["error"] = "Mật khẩu không khớp.";
                    return View(account);
                }

                Random random = new Random();
                int randomNumber = random.Next(10, 9999); // tạo số ngẫu nhiên từ 10 đến 9999
                string accCode = "TK" + randomNumber.ToString();

                // Gán giá trị cho các thuộc tính của account
                account.Password = Encryptor.MD5Hash(account.Password); // mã hóa mật khẩu
                account.CreateDate = DateTime.Now;
                account.RoleID = 3;
                account.Code = accCode;
                account.State=true;

                // Lưu account vào cơ sở dữ liệu
                db.Accounts.Add(account);
                db.SaveChanges();

                // Chuyển hướng đến trang đăng nhập
                return RedirectToAction("Login","Login");
            }

            return View(account);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string pass)
        {
            var encryptedPass = Encryptor.MD5Hash(pass); // mã hóa mật khẩu nhập vào
            var acc = db.Accounts.SingleOrDefault(m => m.Email.Trim().ToLower() == email.Trim().ToLower() && m.Password == encryptedPass);

            if (acc != null)
            {
                Session["user"] = acc;
                FormsAuthentication.SetAuthCookie(acc.Email, false);

                if (acc.RoleID == 1)
                {
                    return RedirectToAction("Index", "Home");
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