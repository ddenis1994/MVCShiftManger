using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace finalProject.Models
{
    public class User
    {
        public string username { get; set; }
        [Key]
        public int userId { get; set; }
        public int Id { get; set; }
        public string password { get; set; }
        public string logInErorMassege { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string gander { get; set; }
        public DateTime birtday { get; set; }
        public DateTime startWork { get; set; }
        public DateTime? EndWork { get; set; }
        public string jobTitle { get; set; }
        public float salary { get; set; }
    }
}