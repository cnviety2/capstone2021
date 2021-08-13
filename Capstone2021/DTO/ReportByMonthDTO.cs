namespace Capstone2021.DTO
{
    //Class show những giá trị về hệ thống trong 1 tháng (từ ngày 1 của tháng đó đến hết tháng
    //,nếu chưa hết tháng thì sẽ từ ngày 1 đến hiện tại
    public class ReportByMonthDTO
    {
        /// <summary>
        /// Số lượng nhà tuyển dụng đăng ký trong 1 tháng
        /// </summary>
        public int numberOfRecruiters { get; set; }

        /// <summary>
        /// Số lượng công việc trong 1 tháng
        /// </summary>
        public int numberOfJobs { get; set; }

        /// <summary>
        /// Số lượng sinh viên nhà tuyển dụng muốn tuyển trong 1 tháng
        /// </summary>
        public int numberOfDesiredStudents { get; set; }

        /// <summary>
        /// Số lượng sinh viên đăng ký trong 1 tháng
        /// </summary>
        public int numberOfStudents { get; set; }

        public string month { get; set; }

    }
}