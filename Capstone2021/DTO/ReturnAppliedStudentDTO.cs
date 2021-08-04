using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone2021.DTO
{
    public class ReturnAppliedStudentDTO
    {
        public int id { get; set; }
        public string gmail { get; set; }

        public ReturnCvDTO cv { get; set; }
    }
}