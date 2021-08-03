﻿using Capstone2021.DTO;
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
                result = context.jobs.AsEnumerable().Where(s => s.id == id).Select(s => JobUtils.mapFromDbContext(s)).FirstOrDefault<Job>();
                result.categories = new List<Category>();
                foreach (job_has_category relationship in result.relationship)
                {
                    Category category = context.categories.AsEnumerable().Where(s => s.code == relationship.category_id).Select(s => new Category()
                    {
                        id = s.id,
                        code = s.code,
                        value = s.value
                    }).FirstOrDefault<Category>();
                    result.categories.Add(category);
                }
            }
            return result;

        }

        public IList<Job> getAll()
        {
            IList<Job> listResult = new List<Job>();
            using (context)
            {
                listResult = context.jobs.AsEnumerable().Select(s => JobUtils.mapFromDbContext(s)).ToList<Job>();
                foreach (Job element in listResult)
                {
                    element.categories = new List<Category>();
                    foreach (job_has_category relationship in element.relationship)
                    {
                        Category category = context.categories.AsEnumerable().Where(s => s.code == relationship.category_id).Select(s => new Category()
                        {
                            id = s.id,
                            code = s.code,
                            value = s.value
                        }).FirstOrDefault<Category>();
                        element.categories.Add(category);
                    }
                }
            }
            return listResult;
        }

        public bool create(CreateJobDTO dto, int recruiterID)
        {
            bool result = false;
            using (context)
            {
                using (var contextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        job model = JobUtils.mapToDatabaseModel(dto);
                        model.recruiter_id = recruiterID;
                        model.string_for_suggestion = JobUtils.getJobSuggestStringFromDTO(dto);
                        context.jobs.Add(model);
                        context.SaveChanges();
                        int jobId = model.id;
                        for (int i = 0; i < dto.categories.Length; i++)
                        {
                            job_has_category relationship = new job_has_category();
                            relationship.category_id = dto.categories[i];
                            relationship.job_id = jobId;
                            relationship.create_date = DateTime.Now;
                            context.job_has_category.Add(relationship);
                        }
                        context.SaveChanges();
                        contextTransaction.Commit();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        contextTransaction.Rollback();
                        logger.Info("Exception " + e.Message + "in JobServiceImpl");
                        return result;
                    }

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
                    checkJob = JobUtils.mapFromDtoToDbModelForUpdating(dto, checkJob);
                    if (checkJob.salary_min > checkJob.salary_max || checkJob.salary_max < checkJob.salary_min)//kiểm tra lại ràng buộc giữa salary max và salary min
                    {
                        return 3;
                    }
                    //checkJob.status = 3; tạm ngưng update lại status của job
                    if (checkJob.job_has_category != null)
                    {
                        context.job_has_category.RemoveRange(context.job_has_category.Where(x => x.job_id == dto.id));
                        foreach (job_has_category ele in checkJob.job_has_category)
                        {
                            ele.create_date = DateTime.Now;
                            context.job_has_category.Add(ele);
                        }
                    }
                    checkJob.string_for_suggestion = JobUtils.getJobSuggestStringFromDTO(checkJob);
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
                result = context.jobs.AsEnumerable().Where(s => s.status == 1).Select(s =>
                    JobUtils.mapFromDbContext(s)

                ).ToList<Job>();

                //xử lý thêm category vào dto trả về cho frontend
                foreach (Job element in result)
                {
                    element.categories = new List<Category>();
                    foreach (job_has_category relationship in element.relationship)
                    {
                        Category category = context.categories.AsEnumerable().Where(s => s.code == relationship.category_id).Select(s => new Category()
                        {
                            id = s.id,
                            code = s.code,
                            value = s.value
                        }).FirstOrDefault<Category>();
                        element.categories.Add(category);
                    }
                }

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
                result = context.jobs.AsEnumerable().Where(s => s.status == 2 && !DateTimeUtils.isOver30Days(s.create_date)).Select(s => JobUtils.mapFromDbContext(s)).ToList<Job>();
                foreach (Job element in result)
                {
                    element.categories = new List<Category>();
                    foreach (job_has_category relationship in element.relationship)
                    {
                        Category category = context.categories.AsEnumerable().Where(s => s.code == relationship.category_id).Select(s => new Category()
                        {
                            id = s.id,
                            code = s.code,
                            value = s.value
                        }).FirstOrDefault<Category>();
                        element.categories.Add(category);
                    }
                }

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
                        student.last_applied_job_string = JobUtils.getJobSuggestStringFromDTO(job);
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

        /// <summary>
        /// Method lọc lại từ list đã lấy dưới database theo đúng search của request gửi lên server
        /// </summary>
        /// <param name="list"></param>
        /// <param name="searchDTO"></param>
        /// <returns></returns>
        private IList<Job> filterAfterGetAllJobsInDatabase(IList<Job> list, SearchJobDTO searchDTO)
        {
            if (searchDTO.keyword != null && !searchDTO.isEmpty())
            {
                list = list.Cast<Job>().Where(s => s.name.Contains(searchDTO.keyword)).ToList<Job>();
            }
            if (searchDTO.categoryCode.HasValue)
            {
                list = list.Cast<Job>().Where(s => s.hasCategory(searchDTO.categoryCode.Value)).ToList<Job>();
            }
            if (searchDTO.location.HasValue)
            {
                list = list.Cast<Job>().Where(s => s.location == searchDTO.location.Value).ToList<Job>();
            }
            if (searchDTO.workingForm.HasValue)
            {
                list = list.Cast<Job>().Where(s => s.workingForm == searchDTO.workingForm.Value).ToList<Job>();
            }
            return list;
        }

        public IList<Job> search(SearchJobDTO searchDTO)
        {
            List<Job> result = new List<Job>();
            using (context)
            {
                result = context.jobs.AsEnumerable().Where(s => s.status == 2 && !DateTimeUtils.isOver30Days(s.create_date)).Select(s => JobUtils.mapFromDbContext(s)).ToList<Job>();
                foreach (Job element in result)
                {
                    element.categories = new List<Category>();
                    foreach (job_has_category relationship in element.relationship)
                    {
                        Category category = context.categories.AsEnumerable().Where(s => s.code == relationship.category_id).Select(s => new Category()
                        {
                            id = s.id,
                            code = s.code,
                            value = s.value
                        }).FirstOrDefault<Category>();
                        element.categories.Add(category);
                    }
                }
            }
            result = (List<Job>)filterAfterGetAllJobsInDatabase(result, searchDTO);
            return result;
        }

        public IList<Job> getSuggestedJob(string studentLastAppliedJobString, int studentId)
        {
            List<Job> result = new List<Job>();
            List<Job> listSearchFromDB = new List<Job>();

            using (context)
            {
                listSearchFromDB = context.jobs.AsEnumerable().Where(s => s.status == 2 &&
                !DateTimeUtils.isOver30Days(s.create_date) && !JobUtils.hasThisStudentApplied(studentId, s)).Select(s => JobUtils.mapFromDbContext(s)).ToList<Job>();
            }
            foreach (Job element in listSearchFromDB)
            {
                if (element.weightCompareToLastAppliedJob(studentLastAppliedJobString) >= 2)
                {
                    result.Add(element);
                }
            }
            if (result.Count > 5)
            {
                result.RemoveRange(5, result.Count - 5);
            }
            return result;
        }

        public IList<Job> getPostedJobByRecruiterId(int recruiterId)
        {
            IList<Job> listResult = new List<Job>();
            using (context)
            {
                listResult = context.jobs.AsEnumerable().Where(s => s.recruiter_id == recruiterId).Select(s => JobUtils.mapFromDbContext(s)).ToList<Job>();
                foreach (Job element in listResult)
                {
                    element.categories = new List<Category>();
                    foreach (job_has_category relationship in element.relationship)
                    {
                        Category category = context.categories.AsEnumerable().Where(s => s.code == relationship.category_id).Select(s => new Category()
                        {
                            id = s.id,
                            code = s.code,
                            value = s.value
                        }).FirstOrDefault<Category>();
                        element.categories.Add(category);
                    }
                }
            }
            return listResult;
        }

        public IList<Job> getAppliedJobByStudentId(int studentId)
        {
            IList<Job> listResult = new List<Job>();
            using (context)
            {
                IList<int> listJobId = context.student_apply_job.AsEnumerable().Where(s => s.student_id == studentId).Select(s => s.job_id).ToList<int>();
                foreach (int jobId in listJobId)
                {
                    listResult.Add(context.jobs.AsEnumerable().Where(s => s.id == jobId).Select(s => JobUtils.mapFromDbContext(s)).FirstOrDefault<Job>());
                }
            }
            return listResult;
        }

        public IList<Category> getAllCategories()
        {
            IList<Category> listResult = new List<Category>();
            using (context)
            {
                listResult = context.categories.Select(s => new Category()
                {
                    id = s.id,
                    code = s.code,
                    value = s.value
                }).ToList<Category>();
            }
            return listResult;
        }
    }
}