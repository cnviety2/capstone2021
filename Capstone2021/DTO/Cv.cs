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
        public string avatar { get; set; }
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
        public DateTime createDate { get; set; }
        /// <summary>
        /// true là được nhận thông báo về email khi có công việc phù hợp, false là không nhận email
        /// </summary>
        public bool isSubscribed { get; set; }

        /// <summary>
        /// True nếu muốn cv public và mọi recruiter có thể tìm thấy cv 
        /// </summary>
        public bool isPublic { get; set; }
    }
}