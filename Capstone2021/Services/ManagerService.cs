using Capstone2021.DTO;
using Capstone2021.Repository.AdminRepository;

namespace Capstone2021.Service
{
    interface ManagerService : IManagerRepository
    {
        ///<summary>
        ///Method login để check login,trả về null nếu không tồn tại user
        ///</summary>
        Manager login(string username, string password);

        ///<summary>
        ///Method update password của admin hoặc staff,return true nếu thành công
        ///</summary>
        bool updatePassword(string password, string username);


        ///<summary>
        ///Method update fullName của admin hoặc staff,return true nếu thành công
        ///</summary>
        bool updateFullName(string fullName, string username);

        /// <summary>
        /// Method ban 1 staff,chỉ role admin mới được sử dụng
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool banAStaff(string username);

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
        bool banAStudent(string username);
    }
}
