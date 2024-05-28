using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace NetFramwork_WildNature.Areas.Admin.Controllers
{
    public class GmailController : Controller
    {
        // GET: Admin/Gmail
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(NetFramwork_WildNature.Models.Gmail model)
        {
            MailMessage mm = new MailMessage(model.From, model.To);
            mm.Subject = model.Sub;
            mm.Body = model.Body;
            mm.IsBodyHtml = false;

            SmtpClient smtp=new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.EnableSsl = true;

            NetworkCredential nc = new NetworkCredential("lukhuong@gmail.com", "khuong190703");
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = nc;  
            smtp.Send(mm);
            ViewBag.Message = "Mail send success !!";
            return View();
        }
    }
}