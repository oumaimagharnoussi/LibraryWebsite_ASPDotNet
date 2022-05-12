using LibraryWebsite.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Library.Controllers
{
    public class AccountController : Controller
    {
        private Models.ApplicationDbContext Context = new Models.ApplicationDbContext();

        // GET: Utilisateur
        public ActionResult Index()
        {
            if ((string)Session["Role"] != "admin")
            {
                return RedirectToAction("Index","Home");
            }
            IList<Models.Utilisateur> UtilisateurList = new List<Models.Utilisateur>();
            var query = from Utilisateur in Context.Utilisateur
                        select Utilisateur;
            var utilisateurs = query.ToList();
            foreach (var UtilisayeurData in utilisateurs)
            {
                UtilisateurList.Add(new Models.Utilisateur()
                {
                    ID = UtilisayeurData.ID,
                    Name = UtilisayeurData.Name,
                    Password = UtilisayeurData.Password,
                    Phone = UtilisayeurData.Phone,
                    Email = UtilisayeurData.Email,
                    Adresse = UtilisayeurData.Adresse,
                    Role = UtilisayeurData.Role,

                });
            }
            return View(UtilisateurList);
        }

        public ActionResult Signup()
        {
            //Models.Utilisateur Utilisateur = new Models.Utilisateur();
            //return View(Utilisateur);
            UtilisateurViewModel utilisateurViewModel = new UtilisateurViewModel();
            return View(utilisateurViewModel);
        }

        [HttpPost]
        public ActionResult Signup(UtilisateurViewModel utilisateurViewModel)
        {
            string role="user";
            if (utilisateurViewModel.Role != null)
                role = utilisateurViewModel.Role;
            try
            {
                Models.Utilisateur Utilisateur = new Models.Utilisateur()
                {
                    ID = utilisateurViewModel.ID,
                    Name = utilisateurViewModel.Name,
                    Password = utilisateurViewModel.Password,
                    Phone = utilisateurViewModel.Phone,
                    Email = utilisateurViewModel.Email,
                    Adresse = utilisateurViewModel.Adresse,
                    Role = role,
                };
                Context.Utilisateur.Add(Utilisateur);
                Context.SaveChanges();
                return RedirectToAction("Index");

            }
            catch
            {
                return View(utilisateurViewModel);
            }
        }

        public ActionResult Edit(int ID=0)
        {
            if (Session["UserID"] == null && ID==0)
            {
                return RedirectToAction("Index", "Home");
            }
            else if (Session["UserID"] != null && ID == 0)
            {
                ID = (int)Session["UserID"];
            }
            
            Models.Utilisateur model = Context.Utilisateur.Where(x => x.ID == ID).ToList().Select(x =>
                                 new Models.Utilisateur()
                                 {
                                     ID = x.ID,
                                     Name = x.Name,
                                     Password = x.Password,
                                     Phone = x.Phone,
                                     Email = x.Email,
                                     Adresse = x.Adresse,
                                     Role = x.Role,
                                 }).SingleOrDefault();
            //Prepare(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Models.Utilisateur model)
        {
            try
            {
                Models.Utilisateur Utilisateur = Context.Utilisateur.Where(x => x.ID == model.ID).Single<Models.Utilisateur>();
                Utilisateur.ID = model.ID;
                Utilisateur.Name = model.Name;
                Utilisateur.Password = model.Password;
                Utilisateur.Phone = model.Phone;
                Utilisateur.Email = model.Email;
                Utilisateur.Adresse = model.Adresse;
                Utilisateur.Role = model.Role;
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
            if ((string)Session["Role"] != "admin")
            {
                return RedirectToAction("Index", "Home");
            }
            Models.Utilisateur model = Context.Utilisateur.Where(x => x.ID == id).ToList().Select(x =>
                                  new Models.Utilisateur()
                                  {
                                      ID = x.ID,
                                      Name = x.Name,
                                      Password = x.Password,
                                      Phone = x.Phone,
                                      Email = x.Email,
                                      Adresse = x.Adresse,
                                      Role = x.Role,
                                  }).SingleOrDefault();
            return View(model);
        }
        [HttpPost]
        public ActionResult Delete(Models.Utilisateur model)
        {
            try
            {
                Models.Utilisateur Utilisateur = Context.Utilisateur.Where(x => x.ID == model.ID).Single<Models.Utilisateur>();
                Context.Utilisateur.Remove(Utilisateur);
                Context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }


        /*private void Prepare(Models.Utilisateur model)
        {
            model.Role = (Models.UtilisateurRole)Context.UtilisateurRole.AsQueryable<Models.UtilisateurRole>().Select(x =>
                    new Models.UtilisateurRole()
                    {
                        RoleName = x.RoleName,
                        ID = x.ID
                    });
        }*/







        public ActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Signin(Models.Utilisateur model)
        {
            /*
            try
            {
                var query=(Models.Utilisateur)from Utilisateur in Context.Utilisateur
                                              where Utilisateur.Name == model.Name select Utilisateur;
                if(query is null)
                {
                    return View(model);
                }
                else if (model.Password != query.Password)
                {
                    return View(model);
                }
                Session.Add(model.Name,model.ID);
                return RedirectToAction("Index");
                
                
            }
            catch
            {
                return View(model);
            }*/


            if (ModelState.IsValid)
            {

                var obj = Context.Utilisateur.Where(x => x.Email.Equals(model.Email) && x.Password.Equals(model.Password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["UserID"] = obj.ID;
                    Session["UserName"] = obj.Name.ToString();
                    Session["Role"] = obj.Role.ToString();
                    return RedirectToAction("Index");
                }

            }
            return View(model);
        }

        public ActionResult Logout()
        {
            //Session.Abandon();
            Session.Clear();
            return RedirectToAction("Index");
        }
    }
}