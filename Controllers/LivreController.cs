using LibraryWebsite.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class LivreController : Controller
    {
        private Models.ApplicationDbContext Context = new Models.ApplicationDbContext();
        public SelectList listThemes;


        // GET: Livre
        public ActionResult Index()
        {
            
            IList<Models.Livre> LivreList= new List<Models.Livre>();
            var query = from Livre in Context.Livre
                        select Livre;
            var Livres = query.ToList();
            foreach (var LivreData in Livres)
            {
                LivreList.Add(new Models.Livre()
                {
                    ID = LivreData.ID,
                    FileName = LivreData.FileName,
                    Title = LivreData.Title,
                    Author = LivreData.Author,
                    Theme = LivreData.Theme,
                    Price= LivreData.Price

                });
            }
            return View(LivreList);
            //return View();

        }

        [HttpPost]
        public ActionResult Index(string titleLivre = null, string Author = null, Models.Theme Theme = null, double Price = 0)
        {
            IList<Models.Livre> LivreList = new List<Models.Livre>();
            var query = from Livre in Context.Livre select Livre;
            var Livres = query.ToList();

            if (titleLivre != null&& titleLivre != "")
            {
                Livres = (List<Models.Livre>)Livres.Where(x => x.Title.Contains(titleLivre)).ToList();
            }
            if (Author != null && Author != "")
            {
                Livres = (List<Models.Livre>)Livres.Where(x => x.Author.Contains(Author)).ToList();
            }
            /*if (Theme != null && Theme != "")
            {
                Livres = (List<Models.Livre>)Livres.Where(x => x.Theme.ID == Theme.ID).ToList();
            }*/
            if (Price != 0)
            {
                Livres = (List<Models.Livre>)Livres.Where(x => x.Price == Price).ToList();
            }

            foreach (var LivreData in Livres)
            {
                LivreList.Add(new Models.Livre()
                {
                    ID = LivreData.ID,
                    FileName = LivreData.FileName,
                    Title = LivreData.Title,
                    Author = LivreData.Author,
                    Theme = LivreData.Theme,
                    Price = LivreData.Price

                });
            }
            return View(LivreList);
        }
        /*
        public ActionResult DownloadFile(int id)
        {
            Models.Livre model = Context.Livre.Where(x => x.ID == id).ToList().Select(x =>
                                  new Models.Livre()
                                  {
                                      ID = x.ID,
                                      FileName = x.FileName,
                                      Title = x.Title,
                                      Author = x.Author,
                                      Theme = x.Theme,
                                      Price = x.Price
                                  }).SingleOrDefault();

            return View(model);
        }*/

        //[HttpPost]
        public ActionResult DownloadFile(string file)
        {
            //Models.Livre Livre = Context.Livre.Where(x => x.ID == model.ID).Single<Models.Livre>();
            //Livre.FileName = model.FileName;
            //string filename = Path.GetFileNameWithoutExtension(model.FileName);
            string path = Path.Combine(Server.MapPath("~/UploadedFiles/"), file);
            System.IO.Path.ChangeExtension(path, null);
            byte[] bytes = System.IO.File.ReadAllBytes(path);


            // Clear Rsponse reference  
            Response.Clear();
            // Add header by specifying file name  
            Response.AddHeader("Content-Disposition", "attachment; filename=" + file);
            // Add header for content length  
            Response.AddHeader("Content-Length", file.Length.ToString());
            // Specify content type  
            Response.ContentType = "text/plain";
            // Clearing flush  
            Response.Flush();
            // Transimiting file  
            Response.TransmitFile(path);
            Response.End();

            //Send the File to Download.
            //File(bytes, "application/octet-stream", file);
            return RedirectToAction("Index");
        }

        public ActionResult Create()
        {

            LivreViewModel Livre = new LivreViewModel();
            /*var m= Context.Theme.Select(x =>
                                new Models.Theme()
                                {
                                    ID = x.ID,
                                    Name = x.Name,
                                });*/
            /*var model = from Theme in Context.Theme
                        select new Models.Theme()
                        {
                            ID = Theme.ID,
                            Name = Theme.Name,
                        };*/
            /*      var  model=m.ToList().Select(x =>
                                new Models.Theme()
                                {
                                    ID = x.ID,
                                    Name = x.Name,
                                });*/
            /*var m = Context.Theme.Where(model => model.Name!=null).ToList().Select(x =>
                                 new Models.Theme()
                                 {
                                     ID = x.ID,
                                     Name = x.Name,
                                 }).SingleOrDefault();
            

            /*foreach (Models.Theme x in m)
                        {
                            listThemes.Append(new SelectListItem { Text = x.Name, Value = x.Name });

                        }*/
            Livre.listThemes=listThemes;
            

            return View(Livre);
        }

        [HttpPost]
        public ActionResult Create(Models.Livre model, HttpPostedFileBase file)
        {
            /*try
            {*/
            string _FileName;
            /*if (file.ContentLength > 0)
            {*/
            _FileName = Path.GetFileName(file.FileName);
            string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);
            file.SaveAs(_path);
            //}

            Models.Livre Livre = new Models.Livre()
            {
                ID = model.ID,
                FileName = _FileName,
                Title = model.Title,
                Author = model.Author,
                Theme = model.Theme,
                Price = model.Price
            };
            Context.Livre.Add(Livre);
            Context.SaveChanges();
            return RedirectToAction("Index");
            /*
        }
        catch
        {
            return View(model);
        }*/
        }

        public ActionResult Edit(int id)
        {
            if ((string)Session["Role"] != "admin" && (string)Session["Role"] != "manager")
            {
                return RedirectToAction("Index", "Home");
            }
            Models.Livre model = Context.Livre.Where(x => x.ID == id).ToList().Select(x =>
                                new Models.Livre()
                                {
                                    ID = x.ID,
                                    FileName = x.FileName,
                                    Title = x.Title,
                                    Author = x.Author,
                                    Theme = x.Theme,
                                    Price = x.Price
                                }).SingleOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Models.Livre model)
        {
            try
            {
                Models.Livre Livre = Context.Livre.Where(x => x.ID == model.ID).Single<Models.Livre>();
                Livre.FileName = model.FileName;
                Livre.Title = model.Title;
                Livre.Author = model.Author;
                Livre.Price = model.Price;
                Livre.Theme = model.Theme;
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
            Models.Livre model = Context.Livre.Where(x => x.ID == id).ToList().Select(x =>
                                  new Models.Livre()
                                  {
                                      ID = x.ID,
                                      FileName = x.FileName,
                                      Title = x.Title,
                                      Author = x.Author,
                                      Theme = x.Theme,
                                      Price = x.Price
                                  }).SingleOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(Models.Livre model)
        {
            /*try
            {*/
                Models.Livre Livre = Context.Livre.Where(x => x.ID == model.ID).Single<Models.Livre>();
                string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), Livre.FileName);
                System.IO.File.Delete(_path);
                Context.Livre.Remove(Livre);
                Context.SaveChanges();
                return RedirectToAction("Index");
            /*}
            catch
            {
                return View(model);
            }*/
        }

        /*
        private void Prepare(Models.Livre model)
        {
            model.Theme = (Models.Theme)Context.Theme.AsQueryable<Models.Theme>().Select(x =>
                    new SelectListItem()
                    {
                        Text = x.Name,
                        Value = x.ID.ToString()
                    });
        }*/

        public ActionResult Search(string Title = null, string Author = null, Models.Theme Theme = null, double Price = 0)
        {
            IList<Models.Livre> LivreList = new List<Models.Livre>();
            var query = from Livre in Context.Livre select Livre;
            var Livres = query.ToList();

            if (Title != null)
            {
                Livres = (List<Models.Livre>)Livres.Where(x => x.Title.Contains(Title));
            }
            if (Author != null)
            {
                Livres = (List<Models.Livre>)Livres.Where(x => x.Author.Contains(Author));
            }
            if (Theme != null)
            {
                Livres = (List<Models.Livre>)Livres.Where(x => x.Theme.ID == Theme.ID);
            }
            if (Price != 0)
            {
                Livres = (List<Models.Livre>)Livres.Where(x => x.Price == Price);
            }

            foreach (var LivreData in Livres)
            {
                LivreList.Add(new Models.Livre()
                {
                    ID = LivreData.ID,
                    Title = LivreData.Title,
                    Author = LivreData.Author,
                    Theme = LivreData.Theme,
                    Price = LivreData.Price

                });
            }
            return View(LivreList);
        }

    }
}