using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NetFramwork_WildNature.Db;

namespace NetFramwork_WildNature.Areas.Admin.Controllers
{
    public class DonatesController : Controller
    {
        private WildNature db = new WildNature();

        // GET: Admin/Donates
        public ActionResult Index()
        {
            var donates = db.Donates.Include(d => d.Account);
            return View(donates.ToList());
        }

        // GET: Admin/Donates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donate donate = db.Donates.Find(id);
            if (donate == null)
            {
                return HttpNotFound();
            }
            return View(donate);
        }

        // GET: Admin/Donates/Create
        public ActionResult Create()
        {
            ViewBag.AccountID = new SelectList(db.Accounts, "ID", "Email");
            return View();
        }

        // POST: Admin/Donates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Amount,Date,AccountID,State")] Donate donate)
        {
            if (ModelState.IsValid)
            {
                db.Donates.Add(donate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountID = new SelectList(db.Accounts, "ID", "Email", donate.AccountID);
            return View(donate);
        }

        // GET: Admin/Donates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donate donate = db.Donates.Find(id);
            if (donate == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountID = new SelectList(db.Accounts, "ID", "Email", donate.AccountID);
            return View(donate);
        }

        // POST: Admin/Donates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Amount,Date,AccountID,State")] Donate donate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountID = new SelectList(db.Accounts, "ID", "Email", donate.AccountID);
            return View(donate);
        }

        // GET: Admin/Donates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donate donate = db.Donates.Find(id);
            if (donate == null)
            {
                return HttpNotFound();
            }
            return View(donate);
        }

        // POST: Admin/Donates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Donate donate = db.Donates.Find(id);
            db.Donates.Remove(donate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
