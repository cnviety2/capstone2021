using Capstone2021.DTO;
using Capstone2021.Repository.AdminRepository;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Capstone2021.Service
{
    /**
     * Class này hiện thực interface AdminService
     */
    public class ManagerServiceImpl : ManagerService, IDisposable
    {
        private DbEntities context;

        public ManagerServiceImpl()
        {
            context = new DbEntities();//DbEntities là class đc Entity Framework tạo ra dùng để kết nối tới db,quản lý db đơn giản hơn
        }
        public bool create(Manager obj)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Manager get(int id)
        {
            Manager result = null;
            using (context)
            {
                //context.admins sẽ lấy ra DataSet của table admins ở phía dưới db
                result = context.managers.AsEnumerable().Where(s => s.id == id).Select(s => new Manager()
                {
                    id = s.id,
                    username = s.username,
                    password = s.password
                }).FirstOrDefault<Manager>();
            }
            return result;
        }

        public List<Manager> getAll()
        {
            throw new NotImplementedException();
        }

        public Manager login(Manager manager)
        {
            Manager result = null;
            using (context)
            {
                Manager checkManager = context.managers.AsEnumerable().Where(s => s.username.Equals(manager.username)).Select(s => new Manager()
                {
                    id = s.id,
                    username = s.username,
                    password = s.password,
                    createDate = s.create_date != null ? s.create_date.Value.ToString("dd/MM/yyyy") : "null",
                    fullName = s.full_name,
                    role = s.role
                }).FirstOrDefault<Manager>();
                if (checkManager == null)
                {
                    return null;
                }
                if (checkManager.password.Equals(manager.password))
                {
                    result = checkManager;
                }
            }
            return result;
        }

        public bool remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool update(Manager obj)
        {
            throw new NotImplementedException();
        }
    }
}