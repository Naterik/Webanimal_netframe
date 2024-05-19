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
    public class DonateController : Controller
    {
        private WildNature db = new WildNature();

        // GET: Admin/Donate
        public ActionResult Index()
        {
            var donates = db.Donates.Include(d => d.Volunteer);
            return View(donates.ToList());
        }

        // GET: Admin/Donate/Details/5
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

        // GET: Admin/Donate/Create
        public ActionResult Create()
        {
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "ID", "Email");
            return View();
        }

        // POST: Admin/Donate/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Amount,Date,VolunteerID,State")] Donate donate)
        {
            if (ModelState.IsValid)
            {
                db.Donates.Add(donate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.VolunteerID = new SelectList(db.Volunteers, "ID", "Email", donate.VolunteerID);
            return View(donate);
        }

        // GET: Admin/Donate/Edit/5
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
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "ID", "Email", donate.VolunteerID);
            return View(donate);
        }

        // POST: Admin/Donate/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Amount,Date,VolunteerID,State")] Donate donate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.VolunteerID = new SelectList(db.Volunteers, "ID", "Email", donate.VolunteerID);
            return View(donate);
        }

        // GET: Admin/Donate/Delete/5
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

        // POST: Admin/Donate/Delete/5
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
