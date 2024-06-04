using System.Linq;
using System.Web.Mvc;
using PagedList;
using NetFramwork_WildNature.Db;

namespace NetFramwork_WildNature.Controllers
{
    public class NewsController : Controller
    {
        private WildNature db = new WildNature();

        // GET: News
        public ActionResult Index(int? page)
        {
            var categories = db.Categories.ToList();
            var news = db.News.OrderByDescending(n => n.Date).ToList();

            ViewBag.Categories = categories;

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(news.ToPagedList(pageNumber, pageSize));
        }

        // GET: News/Details/5
        public ActionResult Details(int id)
        {
            var newsItem = db.News.FirstOrDefault(n => n.ID == id);
            if (newsItem == null)
            {
                return HttpNotFound();
            }

            ViewBag.Categories = db.Categories.ToList();
            return View(newsItem);
        }

        // GET: News/AnimalsByCategory/5
        public ActionResult AnimalsByCategory(int categoryId, int? page)
        {
            var news = db.News.Where(n => n.Animal.CategoryID == categoryId).OrderByDescending(n => n.Date).ToList();
            var category = db.Categories.FirstOrDefault(c => c.ID == categoryId);
            if (category == null)
            {
                return HttpNotFound();
            }

            ViewBag.CategoryName = category.Name;
            ViewBag.Categories = db.Categories.ToList();

            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(news.ToPagedList(pageNumber, pageSize));
        }
    }
}
