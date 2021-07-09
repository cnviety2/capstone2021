using Capstone2021.DTO;
using Capstone2021.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Helpers;

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

        public bool create(Recruiter obj)
        {
            String username = obj.username;
            using (context)
            {
                Recruiter checkRecruiter = context.recruiters.AsEnumerable().Where(s => s.username.Equals(obj.username)).Select(s => new Recruiter()
                {
                    id = s.id,
                    username = s.username
                }).FirstOrDefault<Recruiter>();
                if (checkRecruiter != null)
                {
                    return false;
                }
                else
                {
                    try
                    {
                        recruiter saveObj = new recruiter();
                        saveObj = RecruiterMapper.map(obj);
                        context.recruiters.Add(saveObj);
                        context.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in RecruiterServiceImpl");
                        return false;
                    }
                }
            }
            return true;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Recruiter get(int id)
        {
            throw new NotImplementedException();
        }

        public IList<Recruiter> getAll()
        {
            throw new NotImplementedException();
        }

        /* public Job getAllJob(int id)
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
                     salaryMin = c.salary_min,
                     salaryMax = c.salary_max,
                     offer = c.offer,
                     quantity = c.quantity,
                     workingForm = c.working_form,
                     requirement = c.requirement,
                 }).FirstOrDefault<Job>();
             }
             return result;
         }*/

        public Recruiter login(string username, string password)
        {
            Recruiter result = null;
            using (context)
            {
                Recruiter checkRecruiter = context.recruiters.AsEnumerable().Where(s => s.username.Equals(username)).Select(s => new Recruiter()
                {
                    id = s.id,
                    username = s.username,
                    role = s.role
                }).FirstOrDefault<Recruiter>();
                if (checkRecruiter == null)
                {
                    return null;
                }
                if (Crypto.VerifyHashedPassword(checkRecruiter.password, password))
                {
                    result = checkRecruiter;
                }
            }
            return result;
        }

        public bool remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool update(Recruiter obj)
        {
            throw new NotImplementedException();
        }


    }
}