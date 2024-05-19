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
    public class ConservationStatusController : Controller
    {
        private WildNature db = new WildNature();

        // GET: Admin/ConservationStatus
        public ActionResult Index()
        {
            return View(db.ConservationStatus.ToList());
        }

        // GET: Admin/ConservationStatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConservationStatu conservationStatu = db.ConservationStatus.Find(id);
            if (conservationStatu == null)
            {
                return HttpNotFound();
            }
            return View(conservationStatu);
        }

        // GET: Admin/ConservationStatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/ConservationStatus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Decription")] ConservationStatu conservationStatu)
        {
            if (ModelState.IsValid)
            {
                db.ConservationStatus.Add(conservationStatu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(conservationStatu);
        }

        // GET: Admin/ConservationStatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConservationStatu conservationStatu = db.ConservationStatus.Find(id);
            if (conservationStatu == null)
            {
                return HttpNotFound();
            }
            return View(conservationStatu);
        }

        // POST: Admin/ConservationStatus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Decription")] ConservationStatu conservationStatu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(conservationStatu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(conservationStatu);
        }

        // GET: Admin/ConservationStatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConservationStatu conservationStatu = db.ConservationStatus.Find(id);
            if (conservationStatu == null)
            {
                return HttpNotFound();
            }
            return View(conservationStatu);
        }

        // POST: Admin/ConservationStatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConservationStatu conservationStatu = db.ConservationStatus.Find(id);
            db.ConservationStatus.Remove(conservationStatu);
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
