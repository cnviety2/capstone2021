using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    public class PostAJobDTO
    {
        public int id { get; set; }
        [Required]
        public string jobName { get; set; }
        public int workingForm { get; set; }
        public int location { get; set; }
        public string workingPlace { get; set; }
        public string description { get; set; }
        public string requirement { get; set; }
        public bool type { get; set; }
        public string offer { get; set; }
        public bool sex { get; set;}
        public int quantity { get; set; }
        public int salaryMin { get; set; }
        public int salaryMax { get; set; }
        public int recruiterId { get; set; }
        public DateTime createDate { get; set; }
        public int managerId { get; set; }
    }
}