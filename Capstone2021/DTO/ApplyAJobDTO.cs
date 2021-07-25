using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    public class ApplyAJobDTO
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Id is requried")]
        public Int32 id { get; set; }
    }
}