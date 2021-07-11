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
        bool create(job job, int recruiterID);
    }
}
