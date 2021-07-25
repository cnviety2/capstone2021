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
                result = context.jobs.AsEnumerable().Where(s => s.id == id).Select(s => JobMapper.mapFromDbContext(s)).FirstOrDefault<Job>();
            }
            return result;

        }

        public IList<Job> getAll()
        {
            IList<Job> listResult = new List<Job>();
            using (context)
            {
                listResult = context.jobs.AsEnumerable().Select(s => JobMapper.mapFromDbContext(s)).ToList<Job>();
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

        public bool softRemove(int id)
        {
            throw new NotImplementedException();
        }

        public List<Job> getAllPendingJobs()
        {
            List<Job> result = new List<Job>();
            using (context)
            {
                result = context.jobs.AsEnumerable().Where(s => s.status == 1).Select(s => JobMapper.mapFromDbContext(s)).ToList<Job>();

            }
            return result;
        }

        public bool approveAJob(int jobId, int staffId)
        {
            using (context)
            {
                var job = context.jobs.Find(jobId);
                if (job == null) return false;//ko tìm thấy job
                else
                {
                    if (job.status == 2 || job.manager_id.HasValue) return true;//đã duyệt rồi rồi,duyệt nữa chi ? 
                    else
                    {
                        using (context)
                        {
                            try
                            {
                                job.status = 2;
                                job.manager_id = staffId;
                                job.create_date = DateTime.Now;
                                context.SaveChanges();
                                return true;
                            }
                            catch (Exception e)
                            {
                                logger.Info("Exception " + e.Message + "in JobServiceImpl");
                                return false;
                            }
                        }
                    }
                }
            }
        }

        public List<Job> getAllApprovedJobs()
        {
            List<Job> result = new List<Job>();
            using (context)
            {
                result = context.jobs.AsEnumerable().Where(s => s.status == 2 && !DateTimeUtils.isOver30Days(s.create_date)).Select(s => JobMapper.mapFromDbContext(s)).ToList<Job>();

            }
            return result;
        }

        public int applyAJob(int jobId, int studentId)
        {
            using (context)
            {
                var job = context.jobs.Find(jobId);
                var student = context.students.Find(studentId);
                if (job == null) return 2;
                else
                {
                    if (job.status == 1) return 5;
                    if (DateTimeUtils.isOver30Days(job.create_date)) return 4;
                }
                if (student == null || !student.profile_status) return 3;
                ICollection<student_apply_job> listAppliedJobs = student.student_apply_job;
                if (listAppliedJobs.Any(s => s.job_id == jobId)) return 7;
                else
                {
                    try
                    {
                        student_apply_job relationship = new student_apply_job();
                        relationship.create_date = DateTime.Now;
                        relationship.job_id = jobId;
                        relationship.student_id = studentId;
                        context.student_apply_job.Add(relationship);
                        context.SaveChanges();
                        return 1;
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in JobServiceImpl");
                        return 6;
                    }
                }
            }
        }
    }
}