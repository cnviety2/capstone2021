namespace Capstone2021.DTO
{
    public class ReportyByQuarterDTO
    {
        // <summary>
        /// Số lượng nhà tuyển dụng đăng ký trong 1 năm
        /// </summary>
        public int numberOfRecruiters { get; set; }

        /// <summary>
        /// Số lượng công việc trong 1 năm
        /// </summary>
        public int numberOfJobs { get; set; }

        /// <summary>
        /// Số lượng sinh viên nhà tuyển dụng muốn tuyển trong 1 năm
        /// </summary>
        public int numberOfDesiredStudents { get; set; }

        /// <summary>
        /// Quý của 1 năm,1 : quý 1 (từ tháng 1 -> tháng 3),
        /// 2 : quý 2 (từ tháng 4 -> tháng 6),
        /// 3 : quý 3 (từ tháng 7 -> tháng 9),
        /// 4 : quý 4 (từ tháng 10 -> tháng 12)
        /// </summary>
        public int quarter { get; set; }

        public int year { get; set; }
    }
}