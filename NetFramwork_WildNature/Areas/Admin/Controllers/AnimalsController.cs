using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using NetFramwork_WildNature.Areas.Admin.Models;
using NetFramwork_WildNature.Db;
using static NetFramwork_WildNature.Areas.Admin.Models.AnimalListModel;

namespace NetFramwork_WildNature.Areas.Admin.Controllers
{
    [RoleAuthorize("1", "2")]
    public class AnimalsController : Controller
    {
        private readonly WildNature db = new WildNature();

        // GET: Admin/Animals
        public async Task<ActionResult> Index()
        {
            var animals = db.Animals.Include(a => a.Area).Include(a => a.Category).Include(a => a.Conservation);
            return View(await animals.ToListAsync());
        }

        // GET: Admin/Animals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = new AnimalListModel().GetAnimalDetails(id.Value);
            if (model.Animal == null)
                return HttpNotFound();

            return View(model);
        }

        // GET: Admin/Animals/Create
        public ActionResult Create()
        {
            PopulateViewBags();

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
            PopulateViewBags(animal);
            return View(animal);
        }

        // GET: Admin/Animals/CreateDetail
        public ActionResult CreateDetail()
        {
            if (Session["Animal"] == null)
                return RedirectToAction("Create");

            ViewBag.ColorID = new SelectList(db.Colors, "ID", "Name");
            ViewBag.SpecieID = new SelectList(db.Species, "ID", "Name");
            var model = new List<AnimalDetail> { new AnimalDetail() };  // Initialize with one AnimalDetail
            return View(model);
        }

        // POST: Admin/Animals/CreateDetail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateDetail(List<AnimalDetail> animalDetails, IEnumerable<HttpPostedFileBase> Images)
        {
            if (ModelState.IsValid)
            {
                var animal = Session["Animal"] as Animal;
                if (animal != null)
                {
                    db.Animals.Add(animal);
                    await db.SaveChangesAsync();

                    foreach (var detail in animalDetails)
                    {
                        detail.AnimalID = animal.ID;
                        db.AnimalDetails.Add(detail);
                    }

                    await db.SaveChangesAsync();
                    await SaveImagesAsync(animal.ID, Images);
                    Session["Animal"] = null;
                    return RedirectToAction("Index");
                }
            }
            ViewBag.ColorID = new SelectList(db.Colors, "ID", "Name");
            ViewBag.SpecieID = new SelectList(db.Species, "ID", "Name");
            return View(animalDetails);  
        }


        // GET: Admin/Animals/Edit
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var animal = await db.Animals.FindAsync(id);
            if (animal == null)
                return HttpNotFound();
            ViewBag.ColorID = new SelectList(db.Categories, "ID", "Name");
            ViewBag.AreaID = new SelectList(db.Areas, "ID", "Name");
            ViewBag.ConservationID = new SelectList(db.Conservations, "ID", "Name");
            PopulateViewBags(animal);

            return View(animal);
        }

        // POST: Admin/Animals/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Code,Name,AreaID,CategoryID,State,Decription,ConservationID")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(animal).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("EditDetail", new { id = animal.ID });
            }
            PopulateViewBags(animal);
            return View(animal);
        }


        // GET: Admin/Animals/EditDetail
        public async Task<ActionResult> EditDetail(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var animalDetail = await db.AnimalDetails.FirstOrDefaultAsync(ad => ad.AnimalID == id);
            if (animalDetail == null)
                return HttpNotFound();

            var model = new AnimalListModel.AnimalViewModel
            {
                Animal = await db.Animals.FindAsync(id),
                AnimalDetails = new List<AnimalDetail> { animalDetail },
                Images = await db.Images.Where(img => img.AnimalID == id).ToListAsync()
            };

            ViewBag.ColorID = new SelectList(db.Colors, "ID", "Name", animalDetail.ColorID);
            ViewBag.SpecieID = new SelectList(db.Species, "ID", "Name", animalDetail.SpecieID);

            return View(model);
        }


        // POST: Admin/Animals/EditDetail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditDetail(int id, AnimalDetail animalDetail, IEnumerable<HttpPostedFileBase> images)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingDetail = await db.AnimalDetails.FirstOrDefaultAsync(d => d.ID == animalDetail.ID);
                    if (existingDetail != null)
                    {
                        existingDetail.Weight = animalDetail.Weight;
                        existingDetail.Height = animalDetail.Height;
                        existingDetail.Origin = animalDetail.Origin;
                        existingDetail.ColorID = animalDetail.ColorID;
                        existingDetail.SpecieID = animalDetail.SpecieID;

                        await db.SaveChangesAsync();
                    }

                    await UpdateImagesAsync(id, images);
                    return RedirectToAction("Index");
                }
            }
            catch (OptimisticConcurrencyException)
            {
                // Xử lý ngoại lệ ở đây
                ModelState.AddModelError("", "Dữ liệu đã bị thay đổi từ khi nó được tải. Vui lòng thử lại.");
            }

            var model = new AnimalListModel.AnimalViewModel
            {
                Animal = await db.Animals.FindAsync(id),
                AnimalDetails = new List<AnimalDetail> { animalDetail },
                Images = await db.Images.Where(img => img.AnimalID == id).ToListAsync()
            };

            ViewBag.ColorID = new SelectList(db.Colors, "ID", "Name", animalDetail.ColorID);
            ViewBag.SpecieID = new SelectList(db.Species, "ID", "Name", animalDetail.SpecieID);

            return View(model);
        }


        // DELETE: Admin/Animals/DeleteImage
        [HttpPost]
        public async Task<ActionResult> DeleteImage(int id)
        {
            var image = await db.Images.FindAsync(id);
            if (image != null)
            {
                db.Images.Remove(image);
                await db.SaveChangesAsync();
                var path = Server.MapPath(image.Link);
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        // GET: Admin/Animals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var model = new AnimalListModel().GetAnimalDetails(id.Value);
            if (model.Animal == null)
                return HttpNotFound();

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var animal = await db.Animals.FindAsync(id);
            if (animal == null)
                return HttpNotFound();

            // Xóa tất cả chi tiết của động vật
            var animalDetails = db.AnimalDetails.Where(ad => ad.AnimalID == id);
            db.AnimalDetails.RemoveRange(animalDetails);

            // Xóa tất cả hình ảnh liên quan đến động vật
            var images = db.Images.Where(img => img.AnimalID == id).ToList();
            foreach (var image in images)
            {
                db.Images.Remove(image);
                var path = Server.MapPath(image.Link);
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }

            // Xóa động vật
            db.Animals.Remove(animal);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private void PopulateViewBags(Animal animal = null)
        {
            ViewBag.AreaID = new SelectList(db.Areas, "ID", "Name", animal?.AreaID);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", animal?.CategoryID);
            ViewBag.ConservationID = new SelectList(db.Conservations, "ID", "Name", animal?.ConservationID);
        }

        private async Task SaveImagesAsync(int animalId, IEnumerable<HttpPostedFileBase> images)
        {
            if (images == null) return;

            foreach (var image in images)
            {
                if (image != null && image.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var uniqueFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid()}{Path.GetExtension(fileName)}";
                    var path = Path.Combine(Server.MapPath("~/Asset/Admin/animal_img/"), uniqueFileName);
                    image.SaveAs(path);

                    var newImage = new Image
                    {
                        Name = fileName,
                        FileType = Path.GetExtension(fileName),
                        Link = "~/Asset/Admin/animal_img/" + uniqueFileName,
                        AnimalID = animalId
                    };
                    db.Images.Add(newImage);
                }
            }
            await db.SaveChangesAsync();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }
   

    private async Task UpdateImagesAsync(int animalId, IEnumerable<HttpPostedFileBase> images)
        {
            var existingImages = await db.Images.Where(img => img.AnimalID == animalId).ToListAsync();
            foreach (var existingImage in existingImages)
            {
                db.Images.Remove(existingImage);
                var path = Server.MapPath(existingImage.Link);
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);
            }
            await db.SaveChangesAsync();

            await SaveImagesAsync(animalId, images);
        }

    }
}
