using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    public class UpdatePasswordRecruiterDTO
    {
        [Required]
        [Display(Name = "password")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Password's length minimum is 6 and maximum is 20", MinimumLength = 6)]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
        public string confirmPassword { get; set; }
    }
}