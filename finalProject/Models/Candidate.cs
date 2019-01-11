using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace finalProject.Models
{
    public class Candidate
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string jobTitle { get; set; }
        [Required]
        public string firstName { get; set; }
        [Required]
        public string lastName { get; set; }
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string candidateId { get; set; }
        [Required]
        public string gander { get; set; }
        [Required]
        public DateTime? Birtday { get; set; }
        [Required]
        public string status { get; set; }
    }
}