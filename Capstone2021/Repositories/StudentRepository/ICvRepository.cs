using Capstone2021.DTO;
using Capstone2021.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone2021.Repositories.StudentRepository
{
    ///<summary>
    ///Repository này để làm việc với table CV
    ///</summary>
    interface ICvRepository : IBasicRepository<Cv>
    {
    }
}