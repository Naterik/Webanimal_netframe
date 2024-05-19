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
    public class AnimalController : Controller
    {
        private WildNature db = new WildNature();

        // GET: Admin/Animal
        public ActionResult Index()
        {
            var animals = db.Animals.Include(a => a.Area).Include(a => a.Category).Include(a => a.ConservationStatu);
            return View(animals.ToList());
        }

        // GET: Admin/Animal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // GET: Admin/Animal/Create
        public ActionResult Create()
        {
            ViewBag.AreaID = new SelectList(db.Areas, "ID", "Name");
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.ConservationStatusID = new SelectList(db.ConservationStatus, "ID", "Decription");
            return View();
        }

        // POST: Admin/Animal/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code,Name,AreaID,ConservationStatusID,CategoryID,State,Description")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                db.Animals.Add(animal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreaID = new SelectList(db.Areas, "ID", "Name", animal.AreaID);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", animal.CategoryID);
            ViewBag.ConservationStatusID = new SelectList(db.ConservationStatus, "ID", "Decription", animal.ConservationStatusID);
            return View(animal);
        }

        // GET: Admin/Animal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaID = new SelectList(db.Areas, "ID", "Name", animal.AreaID);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", animal.CategoryID);
            ViewBag.ConservationStatusID = new SelectList(db.ConservationStatus, "ID", "Decription", animal.ConservationStatusID);
            return View(animal);
        }

        // POST: Admin/Animal/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code,Name,AreaID,ConservationStatusID,CategoryID,State,Description")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(animal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaID = new SelectList(db.Areas, "ID", "Name", animal.AreaID);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", animal.CategoryID);
            ViewBag.ConservationStatusID = new SelectList(db.ConservationStatus, "ID", "Decription", animal.ConservationStatusID);
            return View(animal);
        }

        // GET: Admin/Animal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animals.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // POST: Admin/Animal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Animal animal = db.Animals.Find(id);
            db.Animals.Remove(animal);
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
