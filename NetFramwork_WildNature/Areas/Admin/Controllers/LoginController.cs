using NetFramwork_WildNature.Common;
using NetFramwork_WildNature.Db;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;

namespace NetFramwork_WildNature.Areas.Admin.Controllers
{
    public class LoginController : Controller
    {
        private readonly EmailService _emailService;
        private readonly WildNature db = new WildNature();

        public LoginController()
        {
            _emailService = new EmailService();
        }

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

                account.Password = Encryptor.MD5Hash(account.Password); // mã hóa mật khẩu
                account.CreateDate = DateTime.Now;
                account.RoleID = 3;
                account.Code = accCode;
                account.State = true;

                db.Accounts.Add(account);
                db.SaveChanges();

                return RedirectToAction("Login", "Login");
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
            string hashedPassword = Encryptor.MD5Hash(pass); // mã hóa mật khẩu nhập vào
            var acc = db.Accounts.SingleOrDefault(m => m.Email.Trim().ToLower() == email.Trim().ToLower() && m.Password == hashedPassword);

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

        // Forget Password Actions
        public ActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgetPassword(string email)
        {
            var account = db.Accounts.SingleOrDefault(a => a.Email.Trim().ToLower() == email.Trim().ToLower());
            if (account == null)
            {
                TempData["error"] = "Email không tồn tại.";
                return View();
            }

            await SendVerificationCode(email);

            return RedirectToAction("VerifyCode");
        }

        private async Task SendVerificationCode(string email)
        {
            var verificationCode = new Random().Next(100000, 999999).ToString();
            Session["VerificationCode"] = verificationCode;
            Session["VerificationCodeExpire"] = DateTime.Now.AddMinutes(1);
            Session["ResetPasswordEmail"] = email;

            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"] ?? "testemailmvcnet@gmail.com";
            var content = $"Mã xác minh của bạn là: {verificationCode}";

            try
            {
                await _emailService.SendEmailAsync(email, "Mã xác minh", content);
                await _emailService.SendEmailAsync(toEmail, "New contact form submission", content);

                TempData["SuccessMessage"] = "Mã xác minh đã được gửi!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while sending your message: {ex.Message}";
            }
        }

        public ActionResult VerifyCode()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VerifyCode(int code)
        {
            var storedCode = (string)Session["VerificationCode"];
            var expirationTime = (DateTime)Session["VerificationCodeExpire"];

            if (code.ToString() == storedCode && DateTime.Now <= expirationTime)
            {
                Session["IsCodeValid"] = true;
                return RedirectToAction("ResetPassword");
            }

            TempData["error"] = "Invalid or expired code. Please try again.";
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> ResendCode()
        {
            var email = Session["ResetPasswordEmail"]?.ToString();
            if (email != null)
            {
                await SendVerificationCode(email);
                TempData["SuccessMessage"] = "Mã xác minh mới đã được gửi!";
            }
            else
            {
                TempData["error"] = "Không có email nào để gửi lại mã.";
            }

            return RedirectToAction("VerifyCode");
        }

        public ActionResult ResetPassword()
        {
            if (Session["IsCodeValid"] == null || !(bool)Session["IsCodeValid"])
            {
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
            {
                TempData["error"] = "Mật khẩu không khớp.";
                return View();
            }

            var email = Session["ResetPasswordEmail"].ToString();
            var account = db.Accounts.SingleOrDefault(a => a.Email.Trim().ToLower() == email.Trim().ToLower());

            if (account == null)
            {
                TempData["error"] = "Tài khoản không tồn tại.";
                return RedirectToAction("ForgotPassword");
            }

            var newHashedPassword = Encryptor.MD5Hash(newPassword);

            if (db.Accounts.Any(a => a.Password == newHashedPassword))
            {
                TempData["error"] = "Mật khẩu đã tồn tại. Vui lòng nhập mật khẩu khác.";
                return View();
            }

            account.Password = newHashedPassword;
            db.SaveChanges();

            TempData["success"] = "Đặt lại mật khẩu thành công.";
            return RedirectToAction("Login");
        }
    }
}
