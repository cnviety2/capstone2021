using Capstone2021.DTO;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;

namespace Capstone2021.Service
{
    /**
     * Class này hiện thực interface ManagerService
     */
    public class ManagerServiceImpl : ManagerService, IDisposable
    {
        private static Logger logger;
        private DbEntities context;

        public ManagerServiceImpl()
        {
            logger = LogManager.GetCurrentClassLogger();
            context = new DbEntities();//DbEntities là class đc Entity Framework tạo ra dùng để kết nối tới db,quản lý db đơn giản hơn
        }
        public void Dispose()
        {
            context.Dispose();
        }
        public bool create(Manager obj)
        {
            String username = obj.username;
            using (context)
            {
                Manager checkManager = context.managers.AsEnumerable().Where(s => s.username.Equals(obj.username)).Select(s => new Manager()
                {
                    id = s.id,
                    username = s.username
                }).FirstOrDefault<Manager>();
                if (checkManager != null)
                {
                    return false;
                }
                else
                {
                    try
                    {
                        manager saveObj = new manager();
                        saveObj.create_date = DateTime.Now;
                        saveObj.username = obj.username;
                        saveObj.password = obj.password;
                        saveObj.role = obj.role;
                        saveObj.full_name = obj.fullName;
                        context.managers.Add(saveObj);
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in ManagerServiceImpl");
                        return false;
                    }
                }
            }
            return true;
        }

        public Manager get(int id)
        {
            Manager result = null;
            using (context)
            {
                //context.managers sẽ lấy ra DataSet của table manager ở phía dưới db
                result = context.managers.AsEnumerable().Where(s => s.id == id).Select(s => new Manager()
                {
                    id = s.id,
                    username = s.username,
                    password = "***",
                    role = s.role,
                    createDate = s.create_date != null ? s.create_date.Value.ToString("dd/MM/yyyy") : "null",
                    fullName = s.full_name
                }).FirstOrDefault<Manager>();
            }
            return result;
        }

        public IList<Manager> getAll()
        {
            IList<Manager> listResult = new List<Manager>();
            using (context)
            {
                listResult = context.managers.AsEnumerable().Select(s =>
                    new Manager()
                    {
                        id = s.id,
                        createDate = s.create_date != null ? s.create_date.Value.ToString("dd/MM/yyyy") : "null",
                        fullName = s.full_name,
                        username = s.username,
                        role = s.role,
                        password = "***"
                    }
                ).ToList<Manager>();
            }
            return listResult;
        }

        public Manager login(string username, string password)
        {
            Manager result = null;
            using (context)
            {
                Manager checkManager = context.managers.AsEnumerable().Where(s => s.username.Equals(username)).Select(s => new Manager()
                {
                    id = s.id,
                    username = s.username,
                    password = s.password,
                    role = s.role
                }).FirstOrDefault<Manager>();
                if (checkManager == null)
                {
                    return null;
                }
                if (Crypto.VerifyHashedPassword(checkManager.password, password))
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

        public bool updatePassword(string password, string username)
        {
            bool result = false;
            using (context)
            {
                var checkManager = context.managers.SingleOrDefault(b => b.username.Equals(username));
                if (checkManager == null)
                {
                    return result;
                }
                else
                {
                    try
                    {
                        checkManager.password = Crypto.HashPassword(password);
                        context.SaveChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in ManagerServiceImpl");
                    }
                }

            }
            return result;
        }

        public bool updateFullName(string fullName, string username)
        {
            bool result = false;
            using (context)
            {
                var checkManager = context.managers.SingleOrDefault(b => b.username.Equals(username));
                if (checkManager == null)
                {
                    return result;
                }
                else
                {
                    try
                    {
                        checkManager.full_name = fullName;
                        context.SaveChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in ManagerServiceImpl");
                    }
                }

            }
            return result;
        }

        public bool banAStaff(string username)
        {
            throw new NotImplementedException();
        }

        public bool banARecruiter(string username)
        {
            throw new NotImplementedException();
        }

        public bool banAStudent(string username)
        {
            throw new NotImplementedException();
        }
    }
}