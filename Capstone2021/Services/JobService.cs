using Capstone2021.DTO;
using Capstone2021.Repositories.JobRepository;

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
    }
}
