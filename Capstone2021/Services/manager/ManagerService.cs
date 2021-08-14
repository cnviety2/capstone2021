using Capstone2021.DTO;
using Capstone2021.Repository.AdminRepository;
using System.Collections.Generic;

namespace Capstone2021.Service
{
    interface ManagerService : IManagerRepository
    {
        ///<summary>
        ///Method login để check login,trả về null nếu không tồn tại user
        ///</summary>
        Manager login(string username, string password);

        ///<summary>
        ///Method update password của admin hoặc staff,1 : ok, 2 : sai password cũ, 3 : lỗi, 4 : bị ban
        ///</summary>
        int updatePassword(string password, string username, string oldPassword);


        ///<summary>
        ///Method update fullName của admin hoặc staff,return true nếu thành công
        ///</summary>
        bool updateFullName(string fullName, string username);

        /// <summary>
        /// Method ban 1 staff,chỉ role admin mới được sử dụng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool banAStaff(int id);

        bool unbanAStaff(int id);

        /// <summary>
        /// Method ban 1 recruiter,chỉ role admin mới được sử dụng
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool banARecruiter(string username);

        /// <summary>
        /// Method ban 1 student,chỉ role admin mới được sử dụng
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool banAStudent(string gmail);

        /// <summary>
        /// Tạo mới 1 staff,return 1 : ok , 2 : đã tồn tại username , 3 : lỗi 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        int createAStaff(Manager obj);

        ReportByMonthDTO reportByMonth(int month);

        ReportByYearDTO reportByYear();

        ReportyByQuarterDTO reportByQuarter(int quarter);

        /// <summary>
        /// Tạo 1 file excel dựa trên những request nằm bên trong class RequestForReportDTO
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        ComboReportDTO generateReport(RequestForReportDTO dto);

        /// <summary>
        /// Update lại url và ảnh hiển thị trên banner của web chính,staff sử dụng
        /// return 1 : ok , 2 : bị ban , 3 : lỗi, 4 : không tìm thấy banner
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="staffId"></param>
        /// <returns></returns>
        int updateBanner(UpdateBannerDTO dto, int staffId);

        IList<Banner> getAllBanners();

        string updateBannerImgUrl(int id, string imgUrl, int staffId);


    }
}
