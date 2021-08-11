using System.Collections.Generic;

namespace Capstone2021.DTO
{
    public class ReturnStudentDTO
    {
        public int id { get; set; }
        public string gmail { get; set; }
        public string createDate { get; set; }
        public bool profileStatus { get; set; }
        public string avatar { get; set; }
        public string googleId { get; set; }

        public IList<ReturnListCvDTO> listCv { get; set; }
    }
}