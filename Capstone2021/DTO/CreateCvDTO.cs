using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    ///<summary>
    ///Class custom riêng cho việc tạo manager,nhận dữ liệu từ client
    ///</summary>
    public class CreateCvDTO
    {
        [Required(ErrorMessage = "Tên không được thiếu")]
        [StringLength(100, ErrorMessage = "Độ dài của tên lớn hơn 1 ký tự và không quá 100 ký tự", MinimumLength = 1)]
        public string name { get; set; }

        [Required(ErrorMessage = "Tên CV không được thiếu")]
        [StringLength(100, ErrorMessage = "Độ dài của tên lớn hơn 1 ký tự và không quá 100 ký tự", MinimumLength = 1)]
        public string cvName { get; set; }

        [Required(ErrorMessage = "Giới tính không được thiếu")]
        public bool sex { get; set; }

        [Required(ErrorMessage = "Ngày sinh không được thiếu")]
        public string dob { get; set; }

        public String school { get; set; }

        public String experience { get; set; }

        public String foreignLanguage { get; set; }

        [Required(ErrorMessage = "Mức lương mong muốn không được thiếu")]
        [Range(1, Int32.MaxValue, ErrorMessage = "Giá trị nhỏ nhất là 1")]
        public int desiredSalaryMinimum { get; set; }

        [Required(ErrorMessage = "Bán thời gian ? Toàn thời gian ? Cả 2 ?")]
        [Range(1, 3, ErrorMessage = "Part-time:1,Full-time:2,Cả 2:3")]
        public int workingForm { get; set; }

        public String skill { get; set; }

        [Required(ErrorMessage = "SĐT không được thiếu")]
        [RegularExpression("^[0-9]{8,12}$", ErrorMessage = "SĐT chỉ chứa số và không quá 12 số")]
        [StringLength(12, ErrorMessage = "Phone number length minimum is 8 and maximum is 12", MinimumLength = 8)]
        public string phone { get; set; }
    }
}