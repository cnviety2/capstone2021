using System;

namespace Capstone2021.DTO
{
    public class GoogleAuthenDTO
    {
        public String id { get; set; }
        public String email { get; set; }
        public Boolean verified_email { get; set; }
        public String name { get; set; }
        public String given_name { get; set; }
        public String family_name { get; set; }
        public String link { get; set; }
        public String picture { get; set; }

    }
}