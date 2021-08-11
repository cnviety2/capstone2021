using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    //<summary>Class custom riêng cho việc tạo manager,nhận dữ liệu từ client</summary>
    public class CreateManagerDTO
    {
        [Required(ErrorMessage = "Username không được thiếu")]
        [StringLength(20, ErrorMessage = "Độ dài username lớn hơn 6 ký tự và không quá 20 ký tự", MinimumLength = 3)]
        public string username { get; set; }

        [Required(ErrorMessage = "Password không được thiếu")]
        [Display(Name = "password")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Độ dài password lớn hơn 6 ký tự và không quá 20 ký tự", MinimumLength = 6)]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "confirm password")]
        [Compare("password", ErrorMessage = "Password nhập lại chưa đúng")]
        public string confirmPassword { get; set; }

        [Required]
        [RegularExpression("ROLE_ADMIN|ROLE_STAFF", ErrorMessage = "Sai cú pháp của role")]
        public string role { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Độ dài tên không quá 50 ký tự")]
        public string fullName { get; set; }

        public string createDate { get; set; }

    }
}