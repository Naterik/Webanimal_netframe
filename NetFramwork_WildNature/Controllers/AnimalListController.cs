using NetFramwork_WildNature.Areas.Admin.Models;
using NetFramwork_WildNature.Db;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace NetFramwork_WildNature.Controllers
{
    public class AnimalListController : Controller
    {
        private WildNature db = new WildNature();

        // Action Index
        public ActionResult Index(string searchString)
        {
            var images = db.Images.Include(i => i.Animal)
                                  .Include(i => i.Animal.Area)
                                  .Include(i => i.Animal.Conservation);

            if (!String.IsNullOrEmpty(searchString))
            {
                images = images.Where(i => i.Animal.Name.Contains(searchString));
            }

            return View(images.ToList());
        }

        public ActionResult Animal(int id)
        {
            // Use GetAnimalDetails() from AnimalListModel
            var viewModel = new AnimalListModel().GetAnimalDetails(id);

            if (viewModel.Animal == null)
            {
                return HttpNotFound();
            }

            // Pass data to ViewBag
            ViewBag.SelectedImage = viewModel.Images.FirstOrDefault();
            ViewBag.RelatedNews = db.News.Where(n => n.Animal.ID == id).ToList();

            return View(viewModel);
        }
    }
}
