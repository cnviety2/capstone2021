using System;

namespace Capstone2021.DTO
{
    public class Manager
    {
        public int id { get; set; }

        public string username { get; set; }

        public string password { get; set; }

        public string role { get; set; }

        public string fullName { get; set; }

        public string createDate { get; set; }

        public Boolean isBanned { get; set; }

    }
}