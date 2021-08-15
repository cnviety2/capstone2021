using System;

namespace Capstone2021.DTO
{
    public class Student
    {
        public int id { get; set; }
        public string gmail { get; set; }
        public DateTime createDate { get; set; }
        public bool profileStatus { get; set; }
        public String avatar { get; set; }
        public String googleId { get; set; }
        public String lastAppliedJobString { get; set; }
    }
}