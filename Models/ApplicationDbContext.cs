using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Livre> Livre { get; set; }
        public DbSet<Theme> Theme { get; set; }
        public DbSet<Utilisateur> Utilisateur { get; set; }

        public System.Data.Entity.DbSet<LibraryWebsite.ViewModel.UtilisateurViewModel> UtilisateurViewModels { get; set; }
    }
}