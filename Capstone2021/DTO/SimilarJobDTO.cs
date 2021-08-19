using System.Collections.Generic;

namespace Capstone2021.DTO
{
    public class SimilarJobDTO
    {
        public int id { get; set; }

        public string name { get; set; }

        public int salaryMin { get; set; }

        public int salaryMax { get; set; }

        IList<Category> categories { get; set; }
    }
}