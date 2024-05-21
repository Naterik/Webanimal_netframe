using NetFramwork_WildNature.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace NetFramwork_WildNature.Controllers
{
    public class AnimalListController : Controller
    {
        private WildNature db = new WildNature();

        // Action Index
        public ActionResult Index(string searchString)
        {
            var animals = db.Animals.Include(a => a.AnimalDetails.Select(ad => ad.Images))
                                    .Include(a => a.Area)
                                    .Include(a => a.ConservationStatu);

            if (!String.IsNullOrEmpty(searchString))
            {
                animals = animals.Where(a => a.Name.Contains(searchString));
            }

            ViewBag.AnimalDetails = animals.ToList();
            return View(animals.SelectMany(a => a.AnimalDetails.SelectMany(ad => ad.Images)).ToList());
        }


        // Action để hiển thị chi tiết ảnh
        public ActionResult AnimalDetail(int id)
        {
            // Tìm ảnh theo ID và bao gồm các bảng liên quan
            var image = db.Images.Include(i => i.AnimalDetail.Animal)
                                 .Include(i => i.AnimalDetail.Specie)
                                 .Include(i => i.AnimalDetail.Color)
                                 .Include(i => i.AnimalDetail.Animal.Area)
                                 .Include(i => i.AnimalDetail.Animal.ConservationStatu)
                                 .Include(i => i.AnimalDetail.Animal.Category)
                                 .FirstOrDefault(i => i.ID == id);

            if (image == null)
            {
                return HttpNotFound();
            }

            var animalDetail = image.AnimalDetail;
            ViewBag.SelectedImage = image;

            return View(animalDetail);
        }



    }
}
