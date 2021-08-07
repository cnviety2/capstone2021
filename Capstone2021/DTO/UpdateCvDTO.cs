using System;

namespace Capstone2021.DTO
{
    public class UpdateCvDTO
    {
        public int id { get; set; }

        public String name { get; set; }

        public Nullable<Boolean> sex { get; set; }

        public String school { get; set; }

        public String experience { get; set; }

        public String foreignLanguage { get; set; }

        public Nullable<Int32> desiredSalaryMinimum { get; set; }

        public String dob { get; set; }

        public Nullable<Int32> workingForm { get; set; }

        public String cvName { get; set; }

        public String skill { get; set; }

        /// <summary>
        /// Method kiểm tra nếu tất cả các field đều null (trừ field id)
        /// </summary>
        /// <returns></returns>
        public bool isEmpty()
        {
            if (name == null && !sex.HasValue && school == null && experience == null && foreignLanguage == null
                && !desiredSalaryMinimum.HasValue && !workingForm.HasValue && skill == null && dob == null && cvName == null)
                return true;
            else
                return false;
        }
    }
}