using System.ComponentModel.DataAnnotations;
using System.Web.WebPages;

namespace Capstone2021.DTO
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "Username không được thiếu")]
        [StringLength(20, ErrorMessage = "Độ dài username lớn hơn 3 ký tự và không quá 20 ký tự", MinimumLength = 3)]
        public string username { get; set; }

        [Required(ErrorMessage = "Mã lấy lại MK không được thiếu")]
        [StringLength(6, ErrorMessage = "Độ dài mã lấy lại chỉ có 6 ký tự", MinimumLength = 6)]
        public string code { get; set; }

        [Required(ErrorMessage = "Password không được thiếu")]
        [Display(Name = "password")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Độ dài password lớn hơn 6 ký tự và không quá 20 ký tự", MinimumLength = 6)]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "confirm password")]
        [Compare("password", ErrorMessage = "Password nhập lại chưa đúng")]
        public string confirmPassword { get; set; }

        public bool isEmpty()
        {
            if (username.IsEmpty() || code.IsEmpty() || password.IsEmpty() || confirmPassword.IsEmpty())
                return true;
            return false;
        }
    }
}