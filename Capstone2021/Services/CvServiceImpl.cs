using Capstone2021.DTO;
using Capstone2021.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone2021.Services
{
    /**
     * Class này hiện thực interface CvService
     */
    public class CvServiceImpl : CvService, IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private DbEntities context;
        public CvServiceImpl()
        {
            context = new DbEntities();//DbEntities là class đc Entity Framework tạo ra dùng để kết nối tới db,quản lý db đơn giản hơn
        }
        public bool create(Cv obj)
        {
            bool result = false;
            int student_id = obj.studentId;
            cv saveObj = new cv();
            using (context)
            {
                Cv checkCv = context.cvs.AsEnumerable().
                    Where(c => c.student_id.Equals(obj.studentId)).
                    Select(c => new Cv()
                    {
                        studentId = c.student_id,
                        
                    }).FirstOrDefault<Cv>();
                if(checkCv!= null)
                {
                    result = false;
                }
                else
                {
                    try
                    {
                        saveObj = CvMapper.map(obj);
                        context.cvs.Add(saveObj);
                        context.SaveChanges();
                        
                    }
                    catch(Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in CvServiceImpl");
                        result = false;
                    }
                }
            }
            return result;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Cv get(int id)
        {
            Cv result = null;
            using (context)
            {
                //context.cvs sẽ lấy ra DataSet của table cv ở phía dưới db
                result = context.cvs.AsEnumerable()
                    .Where(c => c.student_id == id)
                    .Select(c => CvMapper.getFromDbContext(c))
                    .FirstOrDefault<Cv>();
            }
            return result;
        }

        public IList<Cv> getAll()
        {
            throw new NotImplementedException();
        }

        public bool remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}