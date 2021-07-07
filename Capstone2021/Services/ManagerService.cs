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
    }
}
