using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    public class UpdateFullNameManagerDTO
    {
        [Required]
        [StringLength(50, ErrorMessage = "Full name's maximum length is 50")]
        public string fullName { get; set; }

    }
}