using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using NetFramwork_WildNature.Db;


namespace NetFramwork_WildNature.Areas.Admin.Controllers
{
    public class NewsController : Controller
    {
        // GET: Admin/News
        private WildNature db = new WildNature();
        public ActionResult Index()
        {
            var news = db.News.Include(n => n.Animal);
            return View(news.ToList());
        }

        // GET: Admin/News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: Admin/News/Create
        public ActionResult Create()
        {
            ViewBag.ẠnimalID = new SelectList(db.Animals, "ID", "Name");
            return View();
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "ID,Titile,Images,ẠnimalID,Decription,Date")] News news)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var image = Request.Files["Images"];
                    if (image != null && image.ContentLength > 0)
                    {
                        string imageName = image.FileName;
                        string folder = Server.MapPath("~/Asset/Admin/Image/" + imageName);
                        image.SaveAs(folder);
                        news.Images = "/Asset/Admin/Image/" + imageName;
                    }
                    news.Date = DateTime.Now;

                    db.News.Add(news);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception )
                {
                    // Xử lý ngoại lệ nếu cần
                }
            }
            ViewBag.ẠnimalID = new SelectList(db.Animals, "ID", "Name", news.ẠnimalID);
            return View(news);
        }

        // GET: Admin/News/Edit/5
        public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            ViewBag.ẠnimalID = new SelectList(db.Animals, "ID", "Name", news.ẠnimalID);
            return View(news);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,Titile,Images,ẠnimalID,Decription,Date")] News news)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    News currentNews = db.News.Find(news.ID);
                    var image = Request.Files["Images"];
                    if (image != null && image.ContentLength > 0)
                    {
                        string imageName = image.FileName;
                        string folder = Server.MapPath("~/Asset/Admin/Image/" + imageName);
                        image.SaveAs(folder);
                        news.Images = "/Asset/Admin/Image/" + imageName;
                    }
                    currentNews.Titile = news.Titile;
                    currentNews.Images = news.Images;
                    currentNews.ẠnimalID = news.ẠnimalID;
                    currentNews.Decription = news.Decription;
                    currentNews.Date = news.Date;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch
                {
                    // Handle any errors that might have occurred
                }
            }
            ViewBag.ẠnimalID = new SelectList(db.Animals, "ID", "Name", news.ẠnimalID);
            return View(news);
        }

        // GET: Admin/News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
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
