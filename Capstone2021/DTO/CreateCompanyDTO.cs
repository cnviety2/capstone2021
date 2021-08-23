using System;
using System.ComponentModel.DataAnnotations;
using System.Web.WebPages;

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

        public bool isEmpty() 
        {
            if (name.IsEmpty() && headquaters.IsEmpty() && website.IsEmpty() && description.IsEmpty())
                return true;
            else
                return false;
        }
    }
}