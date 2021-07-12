using System;

namespace Capstone2021.DTO
{
    public class UpdateJobDTO
    {
        public int id { get; set; }

        public String name { get; set; }

        public Nullable<Int32> workingForm { get; set; }

        public Nullable<Int32> location { get; set; }

        public String workingPlace { get; set; }

        public String description { get; set; }

        public String requirement { get; set; }

        public Nullable<Boolean> type { get; set; }

        public String offer { get; set; }

        public Nullable<Int32> sex { get; set; }

        public Nullable<Int32> quantity { get; set; }

        public Nullable<Int32> salaryMin { get; set; }

        public Nullable<Int32> salaryMax { get; set; }

        /// <summary>
        /// Method kiểm tra nếu tất cả các field đều null (trừ field id)
        /// </summary>
        /// <returns></returns>
        public bool isEmpty()
        {
            if (name == null && !workingForm.HasValue && !location.HasValue && workingPlace == null && description == null && requirement == null && !type.HasValue && offer == null
                && !sex.HasValue && !quantity.HasValue && !salaryMin.HasValue && !salaryMax.HasValue)
                return true;
            else
                return false;
        }
    }
}