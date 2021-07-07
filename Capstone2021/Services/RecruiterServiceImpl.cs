using Capstone2021.DTO;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Capstone2021.Services
{
    public class RecruiterServiceImpl : RecruiterService, IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private DbEntities context;
        public RecruiterServiceImpl()
        {
            context = new DbEntities();
        }
        public void Dispose()
        {
           context.Dispose();
        }

        public Job getAllJob(int id)
        {
            Job result = null;
            using (context)
            {
                result = context.jobs.AsEnumerable().Where(c => c.recruiter_id == id).Select(c => new Job()
                {
                    id = c.id,
                    jobName = c.name,
                    recruiterId = c.recruiter_id,
                    createDate = c.create_date,
                    description = c.description,
                    location = c.location,
                    workingPlace = c.working_place,
                    salaryMin=c.salary_min,
                    salaryMax = c.salary_max,
                    offer=c.offer,
                    quantity=c.quantity,
                    workingForm=c.working_form,
                    requirement=c.requirement,
                }).FirstOrDefault<Job>();
            }
            return result;
        }

        public Job PostAJob(PostAJobDTO model)
        {
            throw new NotImplementedException();
        }
    }
}