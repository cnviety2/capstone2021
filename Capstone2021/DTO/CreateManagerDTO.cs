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
        [RegularExpression("^[a-zA-Z0-9_]{6,20}$", ErrorMessage = "Password can only contain alphabet's characters and numbers")]
        [StringLength(20, ErrorMessage = "Password's length minimum is 6 and maximum is 20", MinimumLength = 6)]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "confirm password")]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string confirmPassword { get; set; }

        [Required]
        [RegularExpression("ROLE_ADMIN|ROLE_STAFF", ErrorMessage = "Error syntax of a role")]
        public string role { get; set; }

        [StringLength(50, ErrorMessage = "Full name's maximum length is 50")]
        public string fullName { get; set; }

        public string createDate { get; set; }

    }
}