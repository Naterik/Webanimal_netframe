using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NetFramwork_WildNature.Controllers
{
    public class AnimalListController : Controller
    {
        // GET: AnimalList
        public ActionResult Index()
        {
            return View();
        }
    }
}