using Capstone2021.DTO;
using Capstone2021.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone2021.Services
{
    public class JobServiceImpl : JobService, IDisposable
    {
        private static Logger logger;
        private DbEntities context;

        public JobServiceImpl()
        {
            logger = LogManager.GetCurrentClassLogger();
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

        public bool create(job job, int recruiterID)
        {
            bool result = false;
            using (context)
            {
                try
                {
                    job.recruiter_id = recruiterID;
                    context.jobs.Add(job);
                    context.SaveChanges();
                    result = true;

                }
                catch (Exception e)
                {
                    logger.Info("Exception " + e.Message + "in JobServiceImpl");
                    return result;
                }
            }
            return result;
        }
    }
}