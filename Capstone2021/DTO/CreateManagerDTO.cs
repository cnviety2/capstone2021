using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    //<summary>Class custom riêng cho việc tạo manager,nhận dữ liệu từ client</summary>
    public class CreateManagerDTO
    {
        [Required]
        [StringLength(20, ErrorMessage = "Username's length minimum is 3 and maximum is 20", MinimumLength = 3)]
        public string username { get; set; }

        [Required]
        [Display(Name = "password")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Password's length minimum is 6 and maximum is 20", MinimumLength = 6)]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "confirm password")]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string confirmPassword { get; set; }

        [Required]
        [RegularExpression("ROLE_ADMIN|ROLE_STAFF", ErrorMessage = "Error syntax of a role")]
        public string role { get; set; }

        public string fullName { get; set; }

        public string createDate { get; set; }

    }
}