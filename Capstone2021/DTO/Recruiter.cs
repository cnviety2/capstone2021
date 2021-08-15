using Newtonsoft.Json;
using System;

namespace Capstone2021.DTO
{
    public class Recruiter
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gmail { get; set; }
        public string phone { get; set; }
        public string avatar { get; set; }
        public string createDate { get; set; }
        [JsonIgnore]
        public string role { get; set; }

        /// <summary>
        /// true là nam
        /// </summary>
        public bool sex { get; set; }

        /// <summary>
        /// chưa xài tới
        /// </summary>
        public int status { get; set; }

        public Nullable<Int32> companyId { get; set; }
    }
}