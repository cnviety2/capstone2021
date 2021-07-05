using Capstone2021.DTO;
using Capstone2021.Repository.AdminRepository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone2021.Service
{
    interface ManagerService : IAdminRepository
    {
        ///<summary>
        ///Method login để check login,trả về null nếu không tồn tại user
        ///</summary>
        Manager login(Manager manager);
    }
}
