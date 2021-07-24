using Capstone2021.Repositories.StudentRepository;
using System;

namespace Capstone2021.Services
{
    interface StudentService : StudentRepository, IDisposable
    {
        /// <summary>
        /// Login trả về DTO Student,nếu chưa có sẽ tạo mới rồi trả về
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        DTO.Student login(DTO.Student obj);
    }
}
