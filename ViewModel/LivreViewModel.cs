using Library.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryWebsite.ViewModel
{
    public class LivreViewModel
    {
        private ApplicationDbContext Context = new ApplicationDbContext();
        public int ID { get; set; }

        //public File file;
        public string Title { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }

        public Theme theme { get; set; }

        public SelectList listThemes { get; set; }

    }
}