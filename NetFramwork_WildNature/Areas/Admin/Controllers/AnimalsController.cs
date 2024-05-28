/*using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NetFramwork_WildNature.Areas.Admin.Models;
using NetFramwork_WildNature.Db;
using Newtonsoft.Json.Linq;
using static NetFramwork_WildNature.Areas.Admin.Models.AnimalListModel;

namespace NetFramwork_WildNature.Areas.Admin.Controllers
{
    [RoleAuthorize("1", "2")]
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Code,Name,AreaID,CategoryID,State,Decription,ConservationID")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                Session["Animal"] = animal;
                return RedirectToAction("CreateDetail");
            }

            ViewBag.AreaID = new SelectList(db.Areas, "ID", "Name", animal.AreaID);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", animal.CategoryID);
            ViewBag.ConservationID = new SelectList(db.Conservations, "ID", "Name", animal.ConservationID);
            return View(animal);
        }

        // GET: Admin/Animals/CreateDetail
        public ActionResult CreateDetail()
        {
            if (Session["Animal"] == null)
            {
                return RedirectToAction("Create");
            }

            ViewBag.ColorID = new SelectList(db.Colors, "ID", "Name");
            ViewBag.SpecieID = new SelectList(db.Species, "ID", "Name");
            return View();
            public ActionResult CreateDetailPart2()
            {
                string randomCreateKey = (string)Session["randomCreateKey"];
                AnimalDetail part1Data = (AnimalDetail)Session["createAnimalPart1" + randomCreateKey];
                if (part1Data == null)
                {
                    return RedirectToAction("Create");
                }

                var model = new CreateAnimalDetailsForm();
                model.AnimalDetailList.Add(new AnimalDetail());
                return View(model);
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveDetailPart2(CreateAnimalDetailsForm form, List<HttpPostedFileBase> files)
        {
            string randomCreateKey = (string)Session["randomCreateKey"];
            AnimalDetail part1Data = (AnimalDetail)Session["createAnimalPart1" + randomCreateKey];
            if (part1Data == null)
            {
                return RedirectToAction("Create");
            }

            var animal = new Animal { *//* Set properties from part1Data and form *//* };
            db.Animals.Add(animal);
            db.SaveChanges();

            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        string path = Path.Combine(Server.MapPath("~/uploads"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        var image = new Image { FilePath = path, AnimalID = animal.ID };
                        db.Images.Add(image);
                    }
                }
            }

            db.SaveChanges();
            Session.Remove("randomCreateKey");
            Session.Remove("createAnimalPart1" + randomCreateKey);
            return RedirectToAction("Index");
        }


        // POST: Admin/Animals/CreateDetail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDetail([Bind(Include = "ID,Weight,Height,Origin,ColorID,AnimalID,SpecieID")] AnimalDetail animalDetail, HttpPostedFileBase[] Images)
        {
            if (ModelState.IsValid)
            {
                var animal = Session["Animal"] as Animal;
                if (animal != null)
                {
                    db.Animals.Add(animal);
                    db.SaveChanges();

                    animalDetail.AnimalID = animal.ID;
                    db.AnimalDetails.Add(animalDetail);
                    db.SaveChanges();

                    if (Images != null && Images.Length > 0)
                    {
                        foreach (var image in Images)
                        {
                            if (image != null && image.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(image.FileName);
                                var imagePath = Path.Combine(Server.MapPath("~/Asset/Admin/animal_img/"), fileName);
                                image.SaveAs(imagePath);

                                var newImage = new Image
                                {
                                    Link = "~/Asset/Admin/animal_img/" + fileName,
                                    AnimalID = animal.ID // Sửa lại thành AnimalID
                                };

                                db.Images.Add(newImage);
                                db.SaveChanges();
                            }
                        }
                    }

                    Session["Animal"] = null;
                    return RedirectToAction("Index");
                }
            }

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Code,Name,AreaID,CategoryID,State,Decription,ConservationID")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                Session["Animal"] = animal;
                return RedirectToAction("EditDetail", new { id = animal.ID });
            }
            ViewBag.AreaID = new SelectList(db.Areas, "ID", "Name", animal.AreaID);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", animal.CategoryID);
            ViewBag.ConservationID = new SelectList(db.Conservations, "ID", "Name", animal.ConservationID);
            return View(animal);
        }
        [HttpPost]
        public ActionResult EditDetail(AnimalDetail animalDetail, HttpSession session)
        {
            string randomString = Guid.NewGuid().ToString();
            Session["randomUpdateKey"] = randomString;
            Session["editAnimalPart1" + randomString] = animalDetail;
            return RedirectToAction("EditDetailPart2");
        }


        // GET: Admin/Animals/EditDetail
        public ActionResult EditDetail(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AnimalDetail animalDetail = db.AnimalDetails.FirstOrDefault(ad => ad.AnimalID == id);
            if (animalDetail == null)
            {
                return HttpNotFound();
            }

            ViewBag.ColorID = new SelectList(db.Colors, "ID", "Name", animalDetail.ColorID);
            ViewBag.SpecieID = new SelectList(db.Species, "ID", "Name", animalDetail.SpecieID);
            return View(animalDetail);
        }
        [HttpPost]
        public ActionResult EditDetail(AnimalDetail animalDetail, HttpSession session)
        {
            string randomString = Guid.NewGuid().ToString();
            Session["randomUpdateKey"] = randomString;
            Session["editAnimalPart1" + randomString] = animalDetail;
            return RedirectToAction("EditDetailPart2");
        }
        public ActionResult EditDetailPart2()
        {
            string randomUpdateKey = (string)Session["randomUpdateKey"];
            AnimalDetail part1Data = (AnimalDetail)Session["editAnimalPart1" + randomUpdateKey];
            if (part1Data == null)
            {
                return RedirectToAction("Index");
            }

            var model = new EditAnimalDetailsForm();
            model.AnimalDetailList = db.AnimalDetails.Where(ad => ad.AnimalID == part1Data.AnimalID).ToList();
            return View(model);
        }

        // POST: Admin/Animals/EditDetail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDetail([Bind(Include = "ID,Weight,Height,Origin,ColorID,AnimalID,SpecieID")] AnimalDetail animalDetail, HttpPostedFileBase[] Images)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var animal = Session["Animal"] as Animal;
                    if (animal != null)
                    {
                        // Update the animal entity
                        db.Entry(animal).State = EntityState.Modified;
                        db.SaveChanges();

                        // Update the animalDetail entity
                        animalDetail.AnimalID = animal.ID;
                        var existingAnimalDetail = db.AnimalDetails.Find(animalDetail.ID);
                        if (existingAnimalDetail != null)
                        {
                            existingAnimalDetail.Weight = animalDetail.Weight;
                            existingAnimalDetail.Height = animalDetail.Height;
                            existingAnimalDetail.Origin = animalDetail.Origin;
                            existingAnimalDetail.ColorID = animalDetail.ColorID;
                            existingAnimalDetail.SpecieID = animalDetail.SpecieID;
                            db.Entry(existingAnimalDetail).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        // Handle image uploads
                        if (Images != null && Images.Length > 0)
                        {
                            var existingImages = db.Images.Where(img => img.AnimalID == animal.ID).ToList();

                            // Remove existing images associated with this Animal
                            foreach (var existingImage in existingImages)
                            {
                                db.Images.Remove(existingImage);
                            }
                            db.SaveChanges();

                            foreach (var image in Images)
                            {
                                if (image != null && image.ContentLength > 0)
                                {
                                    var fileName = Path.GetFileName(image.FileName);
                                    var uniqueFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid()}{Path.GetExtension(fileName)}";
                                    var imagePath = Path.Combine(Server.MapPath("~/Asset/Admin/animal_img/"), uniqueFileName);
                                    image.SaveAs(imagePath);

                                    var newImage = new Image
                                    {
                                        Name = fileName,
                                        FileType = Path.GetExtension(fileName),
                                        Link = "~/Asset/Admin/animal_img/" + uniqueFileName,
                                        AnimalID = animal.ID // Sửa lại thành AnimalID
                                    };

                                    db.Images.Add(newImage);
                                }
                            }
                            db.SaveChanges();
                        }

                        Session["Animal"] = null;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return RedirectToAction("Index");
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Unable to save changes. The animal was modified or deleted by another user.");
                }
            }

            ViewBag.ColorID = new SelectList(db.Colors, "ID", "Name", animalDetail.ColorID);
            ViewBag.SpecieID = new SelectList(db.Species, "ID", "Name", animalDetail.SpecieID);
            return View(animalDetail);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveEditDetailPart2(EditAnimalDetailsForm form, List<HttpPostedFileBase> files, List<int> imageRemoveIds)
        {
            string randomUpdateKey = (string)Session["randomUpdateKey"];
            AnimalDetail part1Data = (AnimalDetail)Session["editAnimalPart1" + randomUpdateKey];
            if (part1Data == null)
            {
                return RedirectToAction("Index");
            }

            var animal = db.Animals.Find(part1Data.AnimalID);
            if (animal != null)
            {
                // Update animal details
                db.Entry(animal).CurrentValues.SetValues(part1Data);
                db.SaveChanges();

                // Update images
                var existingImages = db.Images.Where(img => img.AnimalID == animal.ID).ToList();
                foreach (var image in existingImages)
                {
                    if (imageRemoveIds.Contains(image.ID))
                    {
                        db.Images.Remove(image);
                    }
                }

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            string path = Path.Combine(Server.MapPath("~/uploads"), Path.GetFileName(file.FileName));
                            file.SaveAs(path);
                            var image = new Image { FilePath = path, AnimalID = animal.ID };
                            db.Images.Add(image);
                        }
                    }
                }

                db.SaveChanges();
            }

            Session.Remove("randomUpdateKey");
            Session.Remove("editAnimalPart1" + randomUpdateKey);
            return RedirectToAction("Index");
        }


        // Xóa ảnh
        [HttpPost]
        public ActionResult DeleteImage(int id)
        {
            var image = db.Images.Find(id);
            if (image != null)
            {
                db.Images.Remove(image);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        // GET: Admin/Animals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            AnimalListModel animalListModel = new AnimalListModel();
            AnimalListModel.AnimalViewModel model = animalListModel.GetAnimalDetails(id.Value);

            if (model.Animal == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            using (var db = new WildNature())
            {
                // Get the animal
                var animal = db.Animals.Find(id);

                if (animal == null)
                {
                    return HttpNotFound();
                }

                // Get the related animal details
                var animalDetails = db.AnimalDetails.Where(ad => ad.AnimalID == id).ToList();

                // Remove all related animal details
                db.AnimalDetails.RemoveRange(animalDetails);

                // Remove the animal
                db.Animals.Remove(animal);

                db.SaveChanges();
            }

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
*/