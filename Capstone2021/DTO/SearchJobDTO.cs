using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone2021.DTO
{
    public class SearchJobDTO
    {
        public String keyword { get; set; }
        public Nullable<Int32> categoryCode { get; set; }
        public Nullable<Int32> location { get; set; }
        public Nullable<Int32> workingForm { get; set; }

        public Boolean isEmpty()
        {
            if (keyword == null && !categoryCode.HasValue && !location.HasValue && !workingForm.HasValue) return true;
            return false;
        }
    }
}