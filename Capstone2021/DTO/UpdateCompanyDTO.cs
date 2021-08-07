using System;
using System.Web.WebPages;

namespace Capstone2021.DTO
{
    public class UpdateCompanyDTO
    {
        public String name { get; set; }

        public String headquarters { get; set; }

        public String website { get; set; }

        public String description { get; set; }

        public bool isEmpty()
        {
            if (name == null && name.IsEmpty() && headquarters == null && headquarters.IsEmpty() && website == null && website.IsEmpty() && description == null && description.IsEmpty())
            {
                return true;
            }
            return false;
        }
    }
}