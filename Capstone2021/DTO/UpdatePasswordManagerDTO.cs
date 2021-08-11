using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    public class UpdatePasswordManagerDTO
    {
        [Required]
        [Display(Name = "password")]
        [DataType(DataType.Password)]
        [StringLength(20, ErrorMessage = "Độ dài password lớn hơn 6 ký tự và không quá 20 ký tự", MinimumLength = 6)]
        public string password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "confirm password")]
        [Compare("password", ErrorMessage = "Password nhập lại chưa chính xác")]
        public string confirmPassword { get; set; }
    }
}