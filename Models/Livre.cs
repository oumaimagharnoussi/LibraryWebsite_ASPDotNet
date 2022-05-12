using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Library.Models
{
    public class Livre
    {
        public int ID { get; set; }
        public string FileName { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int IDTheme { get; set; }
        public virtual Theme Theme { get; set; }
        public double Price { get; set; }

    }
}