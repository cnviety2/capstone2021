using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    //<summary>
    //Class custom riêng cho việc tạo recruiter,nhận dữ liệu từ client
    //</summary>
    public class CreateRecruiterDTO
    {
        [Required(ErrorMessage = "Username không được thiếu")]
        [StringLength(20, ErrorMessage = "Độ dài username lớn hơn 3 ký tự và không quá 20 ký tự", MinimumLength = 3)]
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
        [Required(ErrorMessage = "Email không được thiếu")]
        [RegularExpression("^[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+$", ErrorMessage = "Email không chính xác. VD: recruiter123@gmail.com")]
        public string gmail { get; set; }
        [Required(ErrorMessage = "SĐT không được thiếu")]
        [RegularExpression("^[0-9]{8,12}$", ErrorMessage = "SĐT chỉ chứa số và không quá 12 số")]
        [StringLength(12, ErrorMessage = "Phone number length minimum is 8 and maximum is 12", MinimumLength = 8)]
        public string phone { get; set; }
        public string createDate { get; set; }
        [Required(ErrorMessage = "Tên không được thiếu")]
        [StringLength(50, ErrorMessage = "Tên không được thiếu", MinimumLength = 1)]
        public string firstname { get; set; }
        [Required(ErrorMessage = "Tên không được thiếu")]
        [StringLength(50, ErrorMessage = "Tên không được thiếu", MinimumLength = 1)]
        public string lastName { get; set; }
        [Required(ErrorMessage = "Giới tính không được thiếu")]
        public bool sex { get; set; }
    }
}