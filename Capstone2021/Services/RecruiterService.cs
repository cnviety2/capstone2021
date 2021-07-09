using Capstone2021.DTO;
using Capstone2021.Repositories.RecruiterRepository;

namespace Capstone2021.Services
{
    interface RecruiterService : IRecruiterRepository
    {
        Recruiter login(string username, string password);

        bool update(Recruiter obj);
    }
}