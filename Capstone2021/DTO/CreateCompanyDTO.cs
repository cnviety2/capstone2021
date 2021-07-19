using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    public class CreateCompanyDTO
    {
        [Required]
        public string name { get; set; }
        [Required]
        public string headquaters { get; set; }
        public String website { get; set; }
        public String description { get; set; }
        public String avatar { get; set; }
    }
}