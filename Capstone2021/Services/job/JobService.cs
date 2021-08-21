using Capstone2021.DTO;
using Capstone2021.Repositories.JobRepository;
using System.Collections.Generic;

namespace Capstone2021.Services
{
    interface JobService : IJobRepository
    {
        /// <summary>
        /// Method tạo mới 1 job,chỉ recruiter mới sử dụng,return -1 lỗi , return lớn hơn 1 : id của job vừa tạo
        /// return -2 : lỗi bên active days
        /// </summary>
        /// <param name="job"></param>
        /// <param name="recruiterID"></param>
        /// <returns></returns>
        int create(CreateJobDTO dto, int recruiterID);

        /// <summary>
        /// Update lại job,return 0 : OK,return 1 : Error,return -1 : không tồn tại,return 2 : đã đc duyệt nên không thể update lại,return 3 : ràng buộc giữa salary max và salary min
        /// 4 : active days chưa đúng
        /// chỉ recruiter mới sử dụng
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        int update(UpdateJobDTO dto, int id);

        /// <summary>
        /// Trả về những job đang ở trạng thái pending,ko quan trọng thời hạn
        /// </summary>
        /// <returns></returns>
        List<ReturnPendingJobDTO> getAllPendingJobs();

        /// <summary>
        /// Trả về những job ở trạng thái OK,đã duyệt và vẫn còn thời hạn trước 30 ngày tính từ lúc job đc approve
        /// </summary>
        /// <returns></returns>
        List<Job> getAllApprovedJobs();

        /// <summary>
        /// Update lại status của job thành 2(đã duyệt),chỉ staff mới sử dụng,trả về true khi đã update thành công
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        bool approveAJob(int jobId, int staffId);

        /// <summary>
        /// Tạo 1 row trong bảng student_apply_job thể hiện student này đã apply vào job này,chỉ student mới sử dụng
        /// trả về: 
        ///   +1 : nếu apply thành công
        ///   +2 : nếu job không tồn tại
        ///   +3 : nếu student không tồn tại
        ///   +4 : nếu job quá hạn
        ///   +5 : nếu job đang ở trạng thái pending
        ///   +6 : nếu lỗi
        ///   +7 : cv không tồn tại
        ///   +8 : cv đã nộp vào job này rồi
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        int applyAJob(int jobId, int studentId, int cvId);

        IList<Job> search(SearchJobDTO searchDTO);

        /// <summary>
        /// Bỏ vào string của job cuối cùng student apply,search trong db những job gần equal như vậy(job đã đc approve và chưa quá 30 ngày)
        /// nếu trong list suggest có job đã đc apply rồi thì sẽ bỏ những job đó đi,chỉ trả về 5 job là nhiều nhất
        /// </summary>
        /// <param name="studentLastAppliedJobString"></param>
        /// <returns></returns>
        IList<Job> getSuggestedJob(string studentLastAppliedJobString, int studentId);

        /// <summary>
        /// Lấy những job đc tạo bởi recruiter này
        /// </summary>
        /// <param name="recruiterId"></param>
        /// <returns></returns>
        IList<Job> getPostedJobByRecruiterId(int recruiterId);

        /// <summary>
        /// Lấy những job đc apply bởi student này
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IList<AppliedJobDTO> getAppliedJobByStudentId(int studentId);

        IList<Category> getAllCategories();

        IList<ActiveDaysAndPrice> getAllActiveDaysAndPrice();

        /// <summary>
        /// Lấy job có paging,chỉ trả về 5 job 1 page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        IList<Job> getAllWithPaging(int page);

        /// <summary>
        /// Trả về tổng số trang nếu mỗi trang là 5 record
        /// </summary>
        /// <returns></returns>
        int getTotalPages();

        /// <summary>
        /// Deny 1 job,update lại status của job đó là 3,4 : bị ban
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        bool denyAJob(int jobId, string message, int staffId);

        /// <summary>
        /// Lấy tất cả những job bị deny của recruiter
        /// </summary>
        /// <param name="recruiterId"></param>
        /// <returns></returns>
        IList<DeniedJobDTO> getAllDeniedJobs(int recruiterId);

        IList<Banner> getAllBanners();

        /// <summary>
        /// Lấy những job tương tự dự trên category của jobId
        /// </summary>
        /// <param name="jobId"></param>
        /// <returns></returns>
        IList<SimilarJobDTO> getSimilarJobs(int jobId);

        IList<Job> getListPartTimeJob(int page);

        int getTotalPagePartTimeJob();

        IList<Job> getListFullTimeJob(int page);

        int getTotalPageFullTimeJob();
    }
}
