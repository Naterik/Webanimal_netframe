using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NetFramwork_WildNature.Areas.Admin.Models;
using NetFramwork_WildNature.Db;
using static NetFramwork_WildNature.Areas.Admin.Models.AnimalListModel;



namespace NetFramwork_WildNature.Areas.Admin.Controllers
{
    public class AnimalsController : Controller
    {
        private WildNature db = new WildNature();

        // GET: Admin/Animals
        public ActionResult Index()
        {
            var animals = db.Animals.Include(a => a.Area).Include(a => a.Category).Include(a => a.Conservation);
            return View(animals.ToList());
        }

        // GET: Admin/Animals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AnimalListModel animalListModel = new AnimalListModel();
            AnimalListModel.AnimalViewModel model = animalListModel.GetAnimalDetails(id.Value);
            _ = animalListModel.GetAnimalDetailsImage(id.Value);

            if (model.Animal == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }


        // GET: Admin/Animals/Create
        public ActionResult Create()
        {
            ViewBag.AreaID = new SelectList(db.Areas, "ID", "Name");
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.ConservationID = new SelectList(db.Conservations, "ID", "Name");
            return View();
        }

        // POST: Admin/Animals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code,Name,AreaID,CategoryID,State,Decription,ConservationID")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                db.Animals.Add(animal);
                db.SaveChanges();
                return RedirectToAction("CreateDetail");
            }

            ViewBag.AreaID = new SelectList(db.Areas, "ID", "Name", animal.AreaID);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", animal.CategoryID);
            ViewBag.ConservationID = new SelectList(db.Conservations, "ID", "Name", animal.ConservationID);
            return View(animal);
        }
        public ActionResult CreateDetail()
        {
            ViewBag.AnimalID = new SelectList(db.Animals, "ID", "Name");
            ViewBag.ColorID = new SelectList(db.Colors, "ID", "Name");
            ViewBag.SpecieID = new SelectList(db.Species, "ID", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetail([Bind(Include = "ID,Weight,Height,Origin,ColorID,AnimalID,SpecieID")] AnimalDetail animalDetail)
        {
            if (ModelState.IsValid)
            {
                db.AnimalDetails.Add(animalDetail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnimalID = new SelectList(db.Animals, "ID", "Name", animalDetail.AnimalID);
            ViewBag.ColorID = new SelectList(db.Colors, "ID", "Name", animalDetail.ColorID);
            ViewBag.SpecieID = new SelectList(db.Species, "ID", "Name", animalDetail.SpecieID);
            return View(animalDetail);
        }

        // GET: Admin/Animals/Edit/5
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
            ViewBag.ConservationID = new SelectList(db.Conservations, "ID", "Name", animal.ConservationID);
            return View(animal);
        }

        // POST: Admin/Animals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code,Name,AreaID,CategoryID,State,Decription,ConservationID")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(animal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaID = new SelectList(db.Areas, "ID", "Name", animal.AreaID);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", animal.CategoryID);
            ViewBag.ConservationID = new SelectList(db.Conservations, "ID", "Name", animal.ConservationID);
            return View(animal);
        }

        // GET: Admin/Animals/Delete/5
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

        // POST: Admin/Animals/Delete/5
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
