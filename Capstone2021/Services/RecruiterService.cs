using Capstone2021.DTO;
using Capstone2021.Repositories.RecruiterRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Capstone2021.Services
{
    interface RecruiterService : IRecruiterRepository
    {
        Job PostAJob(PostAJobDTO model);
        Job getAllJob(int id);
    }
}