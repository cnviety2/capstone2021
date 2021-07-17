using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    //<summary>
    //Class custom riêng cho việc tạo recruiter,nhận dữ liệu từ client
    //</summary>
    public class CreateRecruiterDTO
    {
        [Required]
        [StringLength(20, ErrorMessage = "Username's length minimum is 3 and maximum is 20", MinimumLength = 3)]
        public string username { get; set; }
        [Required]
        [Display(Name = "password")]
        [DataType(DataType.Password)]
        [RegularExpression("^[a-zA-Z0-9_]{6,20}$", ErrorMessage = "Password can only contain alphabet's characters and numbers")]
        [StringLength(20, ErrorMessage = "Password's length minimum is 6 and maximum is 20", MinimumLength = 6)]
        public string password { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "confirm password")]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string confirmPassword { get; set; }
        [Required]
        [RegularExpression("^[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+$", ErrorMessage = "Email Invalid. Example: recruiter123@gmail.com ")]
        public string gmail { get; set; }
        [Required]
        [RegularExpression("^[0-9]{8,12}$", ErrorMessage = "Email Invalid. Example: recruiter123@gmail.com ")]
        [StringLength(12, ErrorMessage = "Phone number length minimum is 8 and maximum is 12", MinimumLength = 8)]
        public string phone { get; set; }
        public string avatar { get; set; }
        public string createDate { get; set; }
    }
}