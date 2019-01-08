using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace finalProject.Models
{
    public class roles
    {
        [Key]
        public int id { get; set; }
        public int userid { get; set; }
        public string role { get; set; }

    }
}