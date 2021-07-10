using Capstone2021.DTO;
using Capstone2021.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone2021.Services
{
    public class JobServiceImpl : JobService, IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private DbEntities context;

        public JobServiceImpl()
        {
            context = new DbEntities();
        }

        public void Dispose()
        {
            context.Dispose();
        }


        public bool create(Job obj)
        {
            throw new NotImplementedException();
        }

        public Job get(int id)
        {
            Job result = null;
            using (context)
            {
                result = context.jobs.AsEnumerable().Where(s => s.id == id).Select(s => JobMapper.getFromDbContext(s)).FirstOrDefault<Job>();
            }
            return result;

        }

        public IList<Job> getAll()
        {
            throw new NotImplementedException();
        }

        public bool remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}