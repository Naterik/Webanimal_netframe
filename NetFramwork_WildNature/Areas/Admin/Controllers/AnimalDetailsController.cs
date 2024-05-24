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
    public class AnimalDetailsController : Controller
    {
        private WildNature db = new WildNature();

        // GET: Admin/AnimalDetails
        public ActionResult Index()
        {
            var animalDetails = db.AnimalDetails.Include(a => a.Animal).Include(a => a.Color).Include(a => a.Specie);
            return View(animalDetails.ToList());
        }

        // GET: Admin/AnimalDetails/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimalDetail animalDetail = db.AnimalDetails.Find(id);
            if (animalDetail == null)
            {
                return HttpNotFound();
            }
            return View(animalDetail);
        }

        // GET: Admin/AnimalDetails/Create
        public ActionResult Create()
        {
            ViewBag.AnimalID = new SelectList(db.Animals, "ID", "Code");
            ViewBag.ColorID = new SelectList(db.Colors, "ID", "Code");
            ViewBag.SpecieID = new SelectList(db.Species, "ID", "Code");
            return View();
        }

        // POST: Admin/AnimalDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Weight,Height,Origin,ColorID,AnimalID,SpecieID")] AnimalDetail animalDetail)
        {
            if (ModelState.IsValid)
            {
                db.AnimalDetails.Add(animalDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnimalID = new SelectList(db.Animals, "ID", "Code", animalDetail.AnimalID);
            ViewBag.ColorID = new SelectList(db.Colors, "ID", "Code", animalDetail.ColorID);
            ViewBag.SpecieID = new SelectList(db.Species, "ID", "Code", animalDetail.SpecieID);
            return View(animalDetail);
        }

        // GET: Admin/AnimalDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimalDetail animalDetail = db.AnimalDetails.Find(id);
            if (animalDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnimalID = new SelectList(db.Animals, "ID", "Code", animalDetail.AnimalID);
            ViewBag.ColorID = new SelectList(db.Colors, "ID", "Code", animalDetail.ColorID);
            ViewBag.SpecieID = new SelectList(db.Species, "ID", "Code", animalDetail.SpecieID);
            return View(animalDetail);
        }

        // POST: Admin/AnimalDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Weight,Height,Origin,ColorID,AnimalID,SpecieID")] AnimalDetail animalDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(animalDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AnimalID = new SelectList(db.Animals, "ID", "Code", animalDetail.AnimalID);
            ViewBag.ColorID = new SelectList(db.Colors, "ID", "Code", animalDetail.ColorID);
            ViewBag.SpecieID = new SelectList(db.Species, "ID", "Code", animalDetail.SpecieID);
            return View(animalDetail);
        }

        // GET: Admin/AnimalDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AnimalDetail animalDetail = db.AnimalDetails.Find(id);
            if (animalDetail == null)
            {
                return HttpNotFound();
            }
            return View(animalDetail);
        }

        // POST: Admin/AnimalDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AnimalDetail animalDetail = db.AnimalDetails.Find(id);
            db.AnimalDetails.Remove(animalDetail);
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
