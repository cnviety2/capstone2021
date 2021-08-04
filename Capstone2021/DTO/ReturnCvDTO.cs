using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone2021.DTO
{
    public class ReturnCvDTO
    {
        public string name { get; set; }
        /// <summary>
        /// Yêu cầu giới tính,true là nam,false là nữ
        /// </summary>
        public bool sex { get; set; }
        public DateTime dob { get; set; }
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
    }
}