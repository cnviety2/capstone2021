using Capstone2021.DTO;
using Capstone2021.Repositories.StudentRepository;
using System;

namespace Capstone2021.Services
{
    interface CvService : ICvRepository
    {
        /// <summary>
        /// Method tạo mới 1 cv,chỉ student mới sử dụng 
        /// </summary>
        /// <param name="cv"></param>
        /// <param name="cvID"></param>
        /// <returns></returns>
        bool create(CreateCvDTO dto, int studentId);
        /// <summary>
        /// Update lại url dẫn đến image của cv
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        bool updateImage(String imageUrl, int id);
        ///<summary>
        ///Method update cv,return true nếu thành công
        ///chỉ student mới đc sử dụng
        ///</summary>
        bool update(UpdateCvDTO dto, int id);
    }
}