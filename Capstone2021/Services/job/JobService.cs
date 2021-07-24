using Capstone2021.DTO;
using Capstone2021.Repositories.JobRepository;
using System.Collections;
using System.Collections.Generic;

namespace Capstone2021.Services
{
    interface JobService : IJobRepository
    {
        /// <summary>
        /// Method tạo mới 1 job,chỉ recruiter mới sử dụng 
        /// </summary>
        /// <param name="job"></param>
        /// <param name="recruiterID"></param>
        /// <returns></returns>
        bool create(CreateJobDTO dto, int recruiterID);

        /// <summary>
        /// Update lại job,return 0 : OK,return 1 : Error,return -1 : không tồn tại,return 2 : đã update nên không thể update lại,return 3 : ràng buộc giữa salary max và salary min
        /// chỉ recruiter mới sử dụng
        /// </summary>
        /// <param name="job"></param>
        /// <returns></returns>
        int update(UpdateJobDTO dto, int id);

        /// <summary>
        /// Trả về những job đang ở trạng thái pending
        /// </summary>
        /// <returns></returns>
        List<Job> getAllPendingJobs();

        /// <summary>
        /// Trả về những job ở trạng thái OK,đã duyệt
        /// </summary>
        /// <returns></returns>
        List<Job> getAllApprovedJobs();

        /// <summary>
        /// Update lại status của job thành 2(đã duyệt),chỉ staff mới sử dụng,trả về true khi đã update thành công
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        bool applyAJob(int jobId, int staffId);
    }
}
