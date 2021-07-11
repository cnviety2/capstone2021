using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    //<summary>
    //Class custom riêng cho việc update thông tin của recruiter,nhận dữ liệu từ client
    //</summary>
    public class UpdateInformationRecruiterDTO
    {
        [Required]
        public int id { get; set; }
        public string companyName { get; set; }
        [Required]
        [RegularExpression("^[A-Za-z0-9+_.-]+@[A-Za-z0-9.-]+$", ErrorMessage = "Email Invalid. Example: recruiter123@gmail.com ")]
        public string gmail { get; set; }
        [Required]
        [RegularExpression("^[0-9]{8,12}$", ErrorMessage = "Phone Invalid")]
        [StringLength(12, ErrorMessage = "Phone number length minimum is 8 and maximum is 12", MinimumLength = 8)]
        public string phone { get; set; }
        [Required]
        public string headquarter { get; set; }
        public string website { get; set; }
        public string description { get; set; }
        public string avatar { get; set; }

    }
}