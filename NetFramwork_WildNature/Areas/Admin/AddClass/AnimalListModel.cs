using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using NetFramwork_WildNature.Db;

namespace NetFramwork_WildNature.Areas.Admin.Models
{
    public class AnimalListModel
    {
        private WildNature db = new WildNature();

        public class AnimalViewModel
        {
            public Animal Animal { get; set; }
            public List<AnimalDetail> AnimalDetails { get; set; } = new List<AnimalDetail>();
            public List<Image> Images { get; set; } = new List<Image>();
        }

        public AnimalViewModel GetAnimalDetails(int id)
        {
            AnimalViewModel model = new AnimalViewModel();
            model.Animal = db.Animals.Include(a => a.Area)
                                      .Include(a => a.Category)
                                      .Include(a => a.Conservation)
                                      .FirstOrDefault(a => a.ID == id);

            if (model.Animal != null)
            {
                model.AnimalDetails = db.AnimalDetails.Where(ad => ad.AnimalID == id)
                                                      .Include(ad => ad.Color)
                                                      .Include(ad => ad.Specie)
                                                      .ToList();

                var detailIds = model.AnimalDetails.Select(ad => ad.ID).ToList();
                model.Images = db.Images.Where(img => detailIds.Contains(img.AnimalDetailID)).ToList();
            }

            return model;
        }
    }
}

