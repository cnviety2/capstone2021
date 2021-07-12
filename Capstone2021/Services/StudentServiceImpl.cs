using Capstone2021.DTO;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace Capstone2021.Services
{
    /**
     * Class này hiện thực interface StudentService
     */
    public class StudentServiceImpl : StudentService, IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private DbEntities context;
        public StudentServiceImpl()
        {
            context = new DbEntities();//DbEntities là class đc Entity Framework tạo ra dùng để kết nối tới db,quản lý db đơn giản hơn
        }
        public bool create(Student obj)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Student get(int id)
        {
            Student result = null;
            using (context)
            {
                //context.student sẽ lấy ra DataSet của table student ở phía dưới db
                result = context.students.AsEnumerable().Where(c => c.id == id).Select(c => new Student()
                {
                    id= c.id,
                    email = c.gmail,
                    phone = c.phone,
                    createDate = c.create_date != null ? c.create_date.ToString("dd/MM/yyyy") : "null",
                    avatar = c.avatar,
                    profileStatus = c.profile_status.ToString(),
                }).FirstOrDefault<Student>();
            }
            return result;
        }

        public IList<Student> getAll()
        {
            IList<Student> listResult = new List<Student>();
            using (context)
            {
                listResult = context.students.AsEnumerable().Select(c => new Student()
                {
                    id = c.id,
                    email = c.gmail,
                    phone = c.phone,
                    createDate = c.create_date != null ? c.create_date.ToString("dd/MM/yyyy") : "null",
                    avatar = c.avatar,
                    profileStatus = c.profile_status.ToString(),
                }).ToList<Student>();
            }
            return listResult;
        }

        public bool remove(int id)
        {
            throw new NotImplementedException();
        }

    }
}