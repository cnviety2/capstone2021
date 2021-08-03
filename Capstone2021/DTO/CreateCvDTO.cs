using System;
using System.ComponentModel.DataAnnotations;

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
        public DateTime dob { get; set; }
        [Required(ErrorMessage = "School you study")]
        public string school { get; set; }
        [Required]
        public string experience { get; set; }
        [Required]
        public string foreignLanguage { get; set; }
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Minimum value is 1")]
        public int desiredSalaryMinimum { get; set; }
        [Required]
        [Range(1, 3, ErrorMessage = "`Part-time:1,Full-time:2,Both:3")]
        public int workingForm { get; set; }
    }
}