using NetFramwork_WildNature.Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NetFramwork_WildNature.Areas.Admin.Models
{
    public class AnimalListModel
    {
        private WildNature db = new WildNature();

        public class AnimalViewModel
        {
            public Animal Animal { get; set; }
            public AnimalDetail AnimalDetail { get; set; }
            public Image Image { get; set; }
        }

        public AnimalViewModel GetAnimalDetails(int id)
        {
            AnimalViewModel model = new AnimalViewModel();
            model.Animal = db.Animals.Find(id);
            model.AnimalDetail = db.AnimalDetails.FirstOrDefault(ad => ad.AnimalID == id);
            return model;
        }
        public AnimalViewModel GetAnimalDetailsImage(int id)
        {
            AnimalViewModel models = new AnimalViewModel();
            models.AnimalDetail = db.AnimalDetails.Find(id);
            models.Image = db.Images.FirstOrDefault(ad => ad.AnimalDetailID == id);
            return models;
        }
    }

}