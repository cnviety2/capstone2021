using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    public class RequestForReportDTO
    {
        [Required]
        [Range(1, 12, ErrorMessage = "Tháng từ 1 -> 12")]
        public int month { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Quý từ 1 -> 4")]
        public int quarter { get; set; }
    }
}