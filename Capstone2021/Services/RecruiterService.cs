using Capstone2021.DTO;
using Capstone2021.Repositories.RecruiterRepository;

namespace Capstone2021.Services
{
    interface RecruiterService : IRecruiterRepository
    {
        /*Job PostAJob(PostAJobDTO model);
        Job getAllJob(int id);*/

        Recruiter login(string username, string password);
    }
}