using System;

namespace Capstone2021.DTO
{
    public class Recruiter
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string companyName { get; set; }
        public string gmail { get; set; }
        public string phone { get; set; }
        public string headquarter { get; set; }
        public string website { get; set; }
        public string description { get; set; }
        public string avatar { get; set; }
        public DateTime createDate { get; set; }

        public string role { get; set; }
    }
}