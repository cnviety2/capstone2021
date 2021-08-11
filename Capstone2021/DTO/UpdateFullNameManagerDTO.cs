using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    public class UpdateFullNameManagerDTO
    {
        [Required]
        [StringLength(50, ErrorMessage = "Độ dài tên không quá 50 ký tự")]
        public string fullName { get; set; }

    }
}