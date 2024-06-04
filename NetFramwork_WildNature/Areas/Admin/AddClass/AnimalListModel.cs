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
            public Area Area { get; set; }
            public Specie Specie { get; set; }
            public Conservation Conservation { get; set; }
            public News News { get; set; }
        }

        public AnimalViewModel GetAnimalDetails(int id)
        {
            AnimalViewModel model = new AnimalViewModel();
            model.Animal = db.Animals.Include(a => a.Area)
                                      .Include(a => a.Category)
                                      .Include(a => a.Conservation)
                                      .Include(a => a.News)
                                      .Include(a => a.Animaldetails.Select(ad => ad.Color))
                                      .Include(a => a.Animaldetails.Select(ad => ad.Specie))
                                      .FirstOrDefault(a => a.ID == id);

            if (model.Animal != null)
            {
                model.AnimalDetails = model.Animal.Animaldetails.ToList();
                model.Images = db.Images.Where(img => img.AnimalID == model.Animal.ID).ToList();
                model.Area = model.Animal.Area;
                model.Conservation = model.Animal.Conservation;
            }

            return model;
        }
    }
}
