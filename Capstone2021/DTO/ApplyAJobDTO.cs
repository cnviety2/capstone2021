using System;
using System.ComponentModel.DataAnnotations;

namespace Capstone2021.DTO
{
    public class ApplyAJobDTO
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Id của công việc không được trống")]
        public Int32 jobId { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Id của cv không được trống")]
        public Int32 cvId { get; set; }
    }
}