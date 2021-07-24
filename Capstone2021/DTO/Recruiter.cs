using System;

namespace Capstone2021.DTO
{
    public class Recruiter
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string fullName { get; set; }
        public string gmail { get; set; }
        public string phone { get; set; }
        public string avatar { get; set; }
        public DateTime createDate { get; set; }
        public string role { get; set; }

        public Boolean isBanned { get; set; }
    }
}