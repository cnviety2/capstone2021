using System;
using System.Web.WebPages;

namespace Capstone2021.DTO
{
    public class UpdateBannerDTO
    {
        public int id { get; set; }

        public String url { get; set; }

        public String imgUrl { get; set; }

        public bool isEmpty()
        {
            if (this.url == null || this.url.IsEmpty())
                return true;
            return false;
        }
    }
}