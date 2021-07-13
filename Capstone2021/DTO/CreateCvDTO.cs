using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone2021.DTO
{
    ///<summary>
    ///Class custom riêng cho việc tạo manager,nhận dữ liệu từ client
    ///</summary>
    public class CreateCvDTO
    {
        [Required]
        [StringLength(100, ErrorMessage = "Name length minimum is 1 and maximum is 100", MinimumLength = 1)]
        public string name { get; set; }
        [Required]
        public bool sex { get; set; }
        [Required]
        public string dob { get; set; }
        [Required]
        public string avatar { get; set; }
        [Required]
        public string school { get; set; }
        [Required]
        public string experience { get; set; }
        public string foreignLanguage { get; set; }
        public int desiredSalaryMinimum { get; set; }
        public int workingForm { get; set; }
        public string createDate { get; set; }
        public bool isSubscribed { get; set; }
    }
}