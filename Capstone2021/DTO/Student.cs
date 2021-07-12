using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone2021.DTO
{
    public class Student
    {
        public int id { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public bool isDelete { get; set; }
        public string createDate { get; set; }
        public string profileStatus { get; set; }
        public string avatar { get; set; }
    }
}