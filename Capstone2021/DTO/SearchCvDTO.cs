using System;

namespace Capstone2021.DTO
{
    public class SearchCvDTO
    {
        public Nullable<Int32> workingForm { get; set; }

        public Nullable<Int32> salary { get; set; }

        public bool isEmpty()
        {
            if (!salary.HasValue && !workingForm.HasValue)
            {
                return true;
            }
            return false;
        }
    }
}