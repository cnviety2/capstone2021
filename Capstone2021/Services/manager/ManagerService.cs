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

        /// <summary>
        /// Lấy data của dashboard,admin xài
        /// </summary>
        /// <returns></returns>
        DashboardDataDTO getDashboardData();

        /// <summary>
        /// Lấy ra list những student dựa theo trang,mỗi lần 5 record,chỉ admin xài
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        IList<Student> getListStudentsWithPaging(int page);

        /// <summary>
        /// Lấy ra list những recruiter dựa theo trang,mỗi lần 5 record,chỉ admin xài
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        IList<ReturnRecruiterForAdminDTO> getListRecruiterWithPaging(int page);

        /// <summary>
        /// Bỏ vào 1 string là student hay recruiter để lấy tổng số trang của table đó
        /// </summary>
        /// <param name="choice"></param>
        /// <returns></returns>
        int getTotalPages(string choice);

        IList<Category> getAllCategory();

        IList<ActiveDaysAndPrice> getAllActiveDaysAndPrice();

        /// <summary>
        /// Tạo mới 1 category,admin xài
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool createACategory(string value);

        /// <summary>
        /// Update 1 category dựa trên id,1 là ok , 2 là không tìm thấy , 3 là lỗi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        int updateACategory(int id, string value);

        /// <summary>
        /// Tạo mới 1 lựa chọn ngày hiệu lực và giá tiền,1 là ok , 2 là giá trị đã lặp lại, 3 là lỗi
        /// </summary>
        /// <param name="days"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        int createAnActiveDaysAndPrice(int days,decimal price);

        /// <summary>
        /// Update 1 lựa chọn ngày hiệu lực và giá tiền dựa trên id,1 là ok, 2 là không tìm thấy , 3 là lỗi ,4 là giá trị đã lặp lại
        /// </summary>
        /// <param name="id"></param>
        /// <param name="days"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        int updateAnActiveDaysAndPrice(UpdateActiveDaysAndPriceDTO dto);

        /// <summary>
        /// Xóa 1 lựa chọn ngày hiệu lực và giá tiền dựa trên id,1 là ok ,2 là ko tìm thấy, 3 là không thể xóa vì chỉ còn lại 2 record dưới db,4 là lỗi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int deleteAnActiceDaysAndPrice(int id);
    }
}
