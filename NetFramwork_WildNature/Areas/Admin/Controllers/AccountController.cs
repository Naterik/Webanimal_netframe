using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NetFramwork_WildNature.Common;
using NetFramwork_WildNature.Db;

namespace NetFramwork_WildNature.Areas.Admin.Controllers
{
    [RoleAuthorize("1")]

    public class AccountController : Controller
    {
        private WildNature db = new WildNature();

        // GET: Admin/Account
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Login", "Login", new { area = "Admin" });
            }
            else
            {
                var accounts = db.Accounts.Include(a => a.Role);
                return View(accounts.ToList());
            }
            
        }

        // GET: Admin/Account/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Admin/Account/Create
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Name");
            return View();
        }

        // POST: Admin/Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Code,Email,Password,CreateDate,RoleID,State")] Account account)
        {
            if (ModelState.IsValid)
            {
                var encryptorPass=Encryptor.MD5Hash(account.Password);
                account.Password = encryptorPass;   
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index","Account");
            }

            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Name", account.RoleID);
            return View(account);
        }

        // GET: Admin/Account/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Name", account.RoleID);
            return View(account);
        }

        // POST: Admin/Account/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Code,Email,Password,CreateDate,RoleID,State")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                var encryptorPass = Encryptor.MD5Hash(account.Password);
                account.Password = encryptorPass;
                db.SaveChanges();
                return RedirectToAction("Index","Account");
            }
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Name", account.RoleID);
            return View(account);
        }

        // GET: Admin/Account/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Admin/Account/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index", "Account");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var account = db.Accounts.Find(id);
            if (account == null)
            {
                return Json(new { success = false });
            }

            account.State = !account.State;
            db.Entry(account).State = EntityState.Modified;
            db.SaveChanges();


            var isLocked = !account.State;
            return Json(new
            {
                success = true,
                id = account.ID,
                state = account.State,
                isLocked = isLocked
            });
        }

    }
}
