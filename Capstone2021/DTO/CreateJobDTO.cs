using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    /// <summary>
    /// Class dùng để nhận data gửi lên từ client custom riêng cho việc tạo 1 job của recruiter
    /// </summary>
    public class CreateJobDTO
    {

        [Required]
        public string name { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "Part-time:1,Full-time:2,Cả 2:3")]
        public int workingForm { get; set; }

        [Required]
        [Range(1, 24, ErrorMessage = "Quận 1 -> 12 : 1 -> 12,13 : bình tân,14 : bình thạnh,15 : gò vấp,16 : phú nhuận,17 : tân bình,18 : " +
            "tân phú,19 : thủ đức,20 : bình chánh,21 : cần giờ,22 : củ chi,23 : hóc môn,24 : nhà bè")]
        public int location { get; set; }

        [Required(ErrorMessage = "Địa chỉ chính xác của nơi làm việc không bỏ trống")]
        public string workingPlace { get; set; }

        [Required]
        public string description { get; set; }

        [Required]
        public string requirement { get; set; }

        [Required(ErrorMessage = "true : 'Việc làm chuyên ngành',false : 'Việc làm phổ thông'")]
        [Range(typeof(bool), "false", "true", ErrorMessage = "true : 'Việc làm chuyên ngành',false : 'Việc làm phổ thông'")]
        public bool type { get; set; }

        [Required]
        public string offer { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "Yêu cầu giới tính : 1 nam,2 nữ,3 cả 2")]
        public Int32 sex { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Giá trị nhỏ nhất là 1")]
        public int quantity { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Giá trị nhỏ nhất là 1")]
        public int salaryMin { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Giá trị nhỏ nhất là 1")]
        public int salaryMax { get; set; }

        [Required]
        public int recruiterId { get; set; }

        [Required]
        public int[] categories { get; set; }

        [Required]
        public int activeDays { get; set; }
    }
}