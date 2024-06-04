using NetFramwork_WildNature.Models;
using NetFramwork_WildNature.Db;
using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NetFramwork_WildNature.Controllers
{
    public class ContactController : Controller
    {
        private readonly EmailService _emailService;
        private readonly WildNature _dbContext;

        public ContactController()
        {
            _emailService = new EmailService();
            _dbContext = new WildNature();
        }

        public ActionResult SendMessage()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendMessage(Gmail model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Check if the email exists in the Account table
            var account = _dbContext.Accounts.FirstOrDefault(a => a.Email == model.To);
            if (account == null)
            {
                TempData["ErrorMessage"] = "The email address does not exist in our records.";
                return View(model);
            }

            string content = System.IO.File.ReadAllText(Server.MapPath("~/Views/Shared/Newcontact.html"));

            content = content.Replace("{{Name}}", model.From);
            content = content.Replace("{{Email}}", model.To);
            content = content.Replace("{{Subject}}", model.Sub);
            content = content.Replace("{{Decription}}", model.Body);

            var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"] ?? "testemailmvcnet@gmail.com";

            try
            {
                await _emailService.SendEmailAsync(model.To, model.Sub, content);
                await _emailService.SendEmailAsync(toEmail, "New contact form submission", content);

                TempData["SuccessMessage"] = "Your message has been sent successfully!";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while sending your message: {ex.Message}";
                return View(model);
            }
        }
    }
}
