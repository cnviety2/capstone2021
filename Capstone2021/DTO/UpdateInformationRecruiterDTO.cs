using System;
using System.Web.WebPages;

namespace Capstone2021.DTO
{
    //<summary>
    //Class custom riêng cho việc update thông tin của recruiter,nhận dữ liệu từ client
    //</summary>
    public class UpdateInformationRecruiterDTO
    {

        public String phone { get; set; }

        public String gmail { get; set; }

        public String lastName { get; set; }

        public String firstName { get; set; }

        public Nullable<Boolean> sex { get; set; }

        public bool isEmpty()
        {
            if ((phone == null && phone.IsEmpty()) && (gmail == null && gmail.IsEmpty())
                && (lastName == null && lastName.IsEmpty()) && (firstName == null && lastName.IsEmpty()) && !sex.HasValue)
            {
                return true;

            }
            else return false;
        }
    }
}