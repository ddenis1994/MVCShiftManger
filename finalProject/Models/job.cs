using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace finalProject.Models
{
    public class Job
    {
        [Key]
        public int id { get; set; }
        public string job { get; set; }
    }
}