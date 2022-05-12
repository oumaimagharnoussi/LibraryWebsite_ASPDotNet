using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LibraryWebsite.ViewModel
{
    public class UtilisateurViewModel
    {
        private ApplicationDbContext Context = new ApplicationDbContext();

        public int ID { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
        public string Adresse { get; set; }


        public string Role { get; set; }

        private List<string> _selectedUtilisateurRole;
        //private List<string> _selectedUtilisateurRole1;
        public List<string> SelectedUtilisateurRole
        {
            get
            {
               //_selectedUtilisateurRole = Context.UtilisateurRole.Select(t => t.RoleName).ToList();
                //foreach (string s in _selectedUtilisateurRole) _selectedUtilisateurRole1.Add(s);


                return _selectedUtilisateurRole;
            }
            set { _selectedUtilisateurRole = value; }
        }
    }
}