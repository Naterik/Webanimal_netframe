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
    [RoleAuthorize("1", "2")]
    public class ConservationsController : Controller
    {
        private WildNature db = new WildNature();

        // GET: Admin/Conservations
        public ActionResult Index()
        {
                return View(db.Conservations.ToList());
        }

        // GET: Admin/Conservations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conservation conservation = db.Conservations.Find(id);
            if (conservation == null)
            {
                return HttpNotFound();
            }
            return View(conservation);
        }

        // GET: Admin/Conservations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Conservations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Decription")] Conservation conservation)
        {
            if (ModelState.IsValid)
            {
                db.Conservations.Add(conservation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(conservation);
        }

        // GET: Admin/Conservations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conservation conservation = db.Conservations.Find(id);
            if (conservation == null)
            {
                return HttpNotFound();
            }
            return View(conservation);
        }

        // POST: Admin/Conservations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Decription")] Conservation conservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(conservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(conservation);
        }

        // GET: Admin/Conservations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Conservation conservation = db.Conservations.Find(id);
            if (conservation == null)
            {
                return HttpNotFound();
            }
            return View(conservation);
        }

        // POST: Admin/Conservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Conservation conservation = db.Conservations.Find(id);
            db.Conservations.Remove(conservation);
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
