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

        public bool remove(int id)
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
            IList<Job> listResult = new List<Job>();
            using (context)
            {
                listResult = context.jobs.AsEnumerable().Select(s => JobMapper.getFromDbContext(s)).ToList<Job>();
            }
            return listResult;
        }

        public bool create(CreateJobDTO dto, int recruiterID)
        {
            bool result = false;
            using (context)
            {
                try
                {
                    job model = JobMapper.mapToDatabaseModel(dto);
                    model.recruiter_id = recruiterID;
                    context.jobs.Add(model);
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

        public int update(UpdateJobDTO dto, int id)
        {
            using (context)
            {
                var checkJob = context.jobs.Find(id);
                if (checkJob == null)
                {
                    return -1;// ko tìm thấy
                }
                if (checkJob.status == 3)
                {
                    return 2;//đã update
                }
                try
                {
                    checkJob = JobMapper.mapFromDtoToDbModelForUpdating(dto, checkJob);
                    if (checkJob.salary_min > checkJob.salary_max || checkJob.salary_max < checkJob.salary_min)//kiểm tra lại ràng buộc giữa salary max và salary min
                    {
                        return 3;
                    }
                    checkJob.status = 3;
                    context.SaveChanges();
                    return 0;//ok
                }
                catch (Exception e)
                {
                    logger.Info("Exception " + e.Message + "in JobServiceImpl");
                }
            }
            return 1;//lỗi
        }

    }
}