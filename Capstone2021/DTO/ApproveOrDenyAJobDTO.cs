using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    public class ApproveOrDenyAJobDTO
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Id là bắt buộc")]
        public Int32 id { get; set; }
    }
}