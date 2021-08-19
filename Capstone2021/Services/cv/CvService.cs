using Capstone2021.DTO;
using Capstone2021.Repositories.StudentRepository;
using System;
using System.Collections.Generic;

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
        string updateImage(String imageUrl, int studentId, int cvId);
        ///<summary>
        ///Method update cv,return 1 : OK,2 : ko tìm thấy cv,3: exception
        ///chỉ student mới đc sử dụng
        ///</summary>
        int update(UpdateCvDTO dto, int studentId);

        /// <summary>
        /// Lấy tất cả cv của student này
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        IList<ReturnListCvDTO> getListCvs(int studentId);

        /// <summary>
        /// Lấy 1 cv của student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="cvId"></param>
        /// <returns></returns>
        Cv get(int studentId, int cvId);

        /// <summary>
        /// Xóa 1 cv của student,return 1 : ok, 2 : cv ko tồn tại, 3 : ko tìm thấy cv, 4 : lỗi,5 : CV này đang được nhà tuyển dụng xem xét
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="cvId"></param>
        /// <returns></returns>
        int removeACv(int studentId, int cvId);

        /// <summary>
        /// Update lại field is_public trong cv của student này, return 1 : ok , 2 : không tồn tại cv này , 3 : đã public rồi , 4 : lỗi
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="cvId"></param>
        /// <returns></returns>
        int publicACv(int studentId, int cvId);

        /// <summary>
        /// Update lại field is_public trong cv của student này, return 1 : ok , 2 : không tồn tại cv này , 3 : đã unpublic rồi , 4 : lỗi
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="cvId"></param>
        /// <returns></returns>
        int unpublicACv(int studentId, int cvId);

        /// <summary>
        /// Tìm kiếm những cv đã được public và chứa từ khóa dựa trên 2 field là cvName và skill,chỉ recruiter xài
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        IList<Cv> searchCvs(SearchCvDTO dto, int page);

        /// <summary>
        /// Trả về tổng số trang dựa trên những options trong dto
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        int getTotalPagesInSearchCv(SearchCvDTO dto);

        /// <summary>
        /// Lấy tất cả những cv đc public
        /// </summary>
        /// <returns></returns>
        IList<Cv> getAllPublicCvs();
    }
}