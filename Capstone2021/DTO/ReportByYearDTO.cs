namespace Capstone2021.DTO
{
    public class ReportByYearDTO
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

        public int year { get; set; }
    }
}