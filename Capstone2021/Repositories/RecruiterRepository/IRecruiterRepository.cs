using Capstone2021.DTO;
using Capstone2021.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone2021.Repositories.RecruiterRepository
{
    ///<summary>
    ///Repository này để làm việc với table Recruiter
    ///</summary>
    interface IRecruiterRepository : IBasicRepository<Recruiter>, IBasicRepository<Job>
    {
    }
    
}