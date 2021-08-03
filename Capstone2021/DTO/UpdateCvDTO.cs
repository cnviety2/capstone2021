using System;

namespace Capstone2021.DTO
{
    public class UpdateCvDTO
    {

        public String name { get; set; }

        public Nullable<Boolean> sex { get; set; }

        public String school { get; set; }

        public String experience { get; set; }

        public String foreignLanguage { get; set; }

        public Nullable<Int32> desiredSalaryMinimum { get; set; }

        public Nullable<Int32> workingForm { get; set; }
        /// <summary>
        /// Method kiểm tra nếu tất cả các field đều null (trừ field id)
        /// </summary>
        /// <returns></returns>
        public bool isEmpty()
        {
            if (name == null && !sex.HasValue && school == null && experience == null && foreignLanguage == null
                && !desiredSalaryMinimum.HasValue && !workingForm.HasValue)
                return true;
            else
                return false;
        }
    }
}