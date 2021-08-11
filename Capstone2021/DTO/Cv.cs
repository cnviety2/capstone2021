using Newtonsoft.Json;
using System;

namespace Capstone2021.DTO
{
    public class Cv
    {
        public int studentId { get; set; }
        public string name { get; set; }
        /// <summary>
        /// Yêu cầu giới tính,true là nam,false là nữ
        /// </summary>
        public bool sex { get; set; }
        public string dob { get; set; }
        public String avatar { get; set; }
        public string school { get; set; }
        /// <summary>
        /// Kinh nghiệm cá nhân
        /// </summary>
        public string experience { get; set; }
        /// <summary>
        /// Các ngôn ngữ được học khác ngoài Tiếng việt
        /// </summary>
        public string foreignLanguage { get; set; }
        /// <summary>
        /// Mức lương yêu cầu được nhận thấp nhất của sinh viên 
        /// </summary>
        public int desiredSalaryMinimum { get; set; }
        /// <summary>
        /// Part time hay full time hay cả 2,1 cho pt,2 cho ft,3 cho cả 2
        /// </summary>
        public int workingForm { get; set; }
        public string createDate { get; set; }
        /// <summary>
        /// True nếu muốn cv public và mọi recruiter có thể tìm thấy cv 
        /// </summary>
        /// 
        [JsonIgnore]
        public bool isPublic { get; set; }

        public string cvName { get; set; }

        public string skill { get; set; }

        public string phone { get; set; }
    }
}