using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class ThemeController : Controller
    {
        private Models.ApplicationDbContext Context = new Models.ApplicationDbContext();

        // GET: Theme
        public ActionResult Index()
        {
            if ((string)Session["Role"] != "admin" && (string)Session["Role"] != "manager")
            {
                return RedirectToAction("Index", "Home");
            }
            IList<Models.Theme> ThemeList = new List<Models.Theme>();
            var query = from Theme in Context.Theme
                        select Theme;
            var Themes = query.ToList();
            foreach (var ThemeData in Themes)
            {
                ThemeList.Add(new Models.Theme()
                {
                    ID = ThemeData.ID,
                    Name = ThemeData.Name

                });
            }
            return View(ThemeList);
        }

        public ActionResult Create()
        {
            if ((string)Session["Role"] != "admin" && (string)Session["Role"] != "manager")
            {
                return RedirectToAction("Index", "Home");
            }
            Models.Theme Theme = new Models.Theme();
            return View(Theme);
        }

        [HttpPost]
        public ActionResult Create(Models.Theme model)
        {
            try
            {
                Models.Theme Theme = new Models.Theme()
                {
                    ID = model.ID,
                    Name = model.Name
                };
                Context.Theme.Add(Theme);
                Context.SaveChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            if ((string)Session["Role"] != "admin" && (string)Session["Role"] != "manager")
            {
                return RedirectToAction("Index", "Home");
            }
            Models.Theme model = Context.Theme.Where(x => x.ID == id).ToList().Select(x =>
                                new Models.Theme()
                                {
                                    ID = x.ID,
                                    Name = x.Name
                                }).SingleOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Models.Theme model)
        {
            try
            {
                Models.Theme Theme = Context.Theme.Where(x => x.ID == model.ID).Single<Models.Theme>();
                Theme.Name = model.Name;
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
        public ActionResult Delete(int id)
        {
            if ((string)Session["Role"] != "admin" && (string)Session["Role"] != "manager")
            {
                return RedirectToAction("Index", "Home");
            }
            Models.Theme model = Context.Theme.Where(x => x.ID == id).ToList().Select(x =>
                                  new Models.Theme()
                                  {
                                      ID = x.ID,
                                      Name = x.Name
                                  }).SingleOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(Models.Theme model)
        {
            try
            {
                Models.Theme Theme = Context.Theme.Where(x => x.ID == model.ID).Single<Models.Theme>();
                Context.Theme.Remove(Theme);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }


    }
}