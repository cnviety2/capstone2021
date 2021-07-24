using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Capstone2021.DTO
{
    public class ApproveAJobDTO
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Id is requried")]
        public Int32 id { get; set; }
    }
}