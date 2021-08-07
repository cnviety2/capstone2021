using Capstone2021.DTO;
using Capstone2021.Repositories.CompanyRepository;
using System;

namespace Capstone2021.Services
{
    interface CompanyService : CompanyRepository
    {
        /// <summary>
        /// Method tạo mới 1 company,recruiter mới được xài,return 1 : OK,2 : đã tạo rồi,3 : lỗi
        /// </summary>
        /// <param name="model"></param>
        /// <param name="recruiterId"></param>
        /// <returns></returns>
        int createNewCompanyInfo(CreateCompanyDTO model, int recruiterId);


        /// <summary>
        /// Trả về company data của recruiter này
        /// </summary>
        /// <param name="recruiterId"></param>
        /// <returns></returns>
        Company getSelfCompany(int recruiterId);

        /// <summary>
        /// Update lại thông tin công ty của 1 recruiter,return 1 : OK , 2 : không tồn tại recruiter, 3 : recruiter chưa có company , 4 : lỗi
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="recuiterId"></param>
        /// <returns></returns>
        int updateCompanyInfo(UpdateCompanyDTO dto, int recruiterId);

        ReturnCompanyDTO getCompanyByRecruiterId(int recuiterId);

        /// <summary>
        /// Update lại url dẫn đến image của company
        /// </summary>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        string updateImage(String imageUrl, int recruiterId);
    }
}
