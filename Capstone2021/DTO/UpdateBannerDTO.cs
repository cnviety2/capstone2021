using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace Capstone2021.DTO
{
    public class UpdateBannerDTO
    {
        [Required]
        [Range(1, 4, ErrorMessage = "Từ 1 -> 4")]
        public int id { get; set; }

        public String url { get; set; }

        public bool isEmpty()
        {
            if (this.url == null || this.url.IsEmpty())
                return true;
            return false;
        }
    }
}