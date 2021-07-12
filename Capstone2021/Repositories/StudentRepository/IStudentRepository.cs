using Capstone2021.DTO;
using Capstone2021.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone2021.Repositories.StudentRepository
{

    ///<summary>
    ///Repository này để làm việc với table Student
    ///</summary>
    interface IStudentRepository : IBasicRepository<Student>
    {
    }
}