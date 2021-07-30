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


        /// <summary>
        /// Update lại url dẫn đến image của student
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        bool updateImage(String imageUrl, int id);


        /// <summary>
        /// Trả về lastAppliedJobString của student
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        string getLastAppliedJobString(int studentId);
    }
}
