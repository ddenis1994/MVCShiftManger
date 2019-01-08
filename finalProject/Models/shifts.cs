using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace finalProject.Models
{
    public class shifts
    {
        public class shift
        {
            [Key]
            public int shiftId { get; set; }
            public int shiftsId { get; set; }
            public string shiftChose { get; set; }
            public string date { get; set; }

        }
        [Key]
        public int shiftsId { get; set; }
        public string startDate { get; set; }
        public int week { get; set; }
        public int year { get; set; }
        public int userId { get; set; }
        public ICollection<shift> shiftList{ get; set; }

    }
}