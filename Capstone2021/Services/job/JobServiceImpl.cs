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
                result = context.jobs.AsEnumerable().Where(s => s.id == id).Select(s => JobUtils.mapFromDbContext(s)).FirstOrDefault<Job>();
                if (DateTimeUtils.isOverAfterDays(result.createDate2, result.activeDays))
                {
                    result.isOver = true;
                }
                else result.isOver = false;
                result.endDate = result.createDate2.AddDays(result.activeDays).ToString("dd/MM/yyyy");
                result.categories = new List<Category>();
                foreach (job_has_category relationship in result.relationship)
                {
                    Category category = context.categories.AsEnumerable().Where(s => s.id == relationship.category_id).Select(s => new Category()
                    {
                        id = s.id,
                        value = s.value
                    }).FirstOrDefault<Category>();
                    result.categories.Add(category);
                }
                var job = context.jobs.Find(id);
                result.imgUrl = job.recruiter.avatar;
            }
            return result;

        }

        public IList<Job> getAll()
        {
            IList<Job> listResult = new List<Job>();
            using (context)
            {
                listResult = context.jobs.AsEnumerable().Where(s => s.status == 2 && !DateTimeUtils.isOverAfterDays(s.create_date, s.active_days))
                   .Select(s => JobUtils.mapFromDbContext(s))
                   .OrderByDescending(s => s.createDate2)
                   .ToList<Job>();
                foreach (Job element in listResult)
                {
                    element.endDate = element.createDate2.AddDays(element.activeDays).ToString("dd/MM/yyyy");
                    element.categories = new List<Category>();
                    foreach (job_has_category relationship in element.relationship)
                    {
                        Category category = context.categories.AsEnumerable().Where(s => s.id == relationship.category_id).Select(s => new Category()
                        {
                            id = s.id,
                            value = s.value
                        }).FirstOrDefault<Category>();
                        element.categories.Add(category);
                    }
                }
                foreach (Job dto in listResult)
                {
                    var company = context.companies.Where(s => s.recruiter_id == dto.recruiterId).FirstOrDefault();
                    if (company != null)
                    {
                        dto.imgUrl = company.avatar;
                    }
                    else
                    {
                        dto.imgUrl = "";
                    }
                }
            }
            return listResult;
        }

        public int create(CreateJobDTO dto, int recruiterID)
        {
            int result = -1;
            using (context)
            {
                using (var contextTransaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        job model = JobUtils.mapToDatabaseModel(dto);
                        var checkActiveDays = context.active_days_price.Where(s => s.active_days == model.active_days).FirstOrDefault();
                        if (checkActiveDays == null)
                        {
                            return -2;
                        }
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
                        result = jobId;
                        return result;
                    }
                    catch (Exception e)
                    {
                        contextTransaction.Rollback();
                        logger.Info("Exception " + e.Message + "in JobServiceImpl");
                        return -1;
                    }

                }
            }
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
                if (checkJob.status == 2)
                {
                    return 2;//đã đc duyệt
                }
                try
                {
                    if (dto.activeDays.HasValue)
                    {
                        var checkActiveDays = context.active_days_price.Where(s => s.active_days == dto.activeDays.Value).FirstOrDefault();
                        if (checkActiveDays == null)
                        {
                            return 4;
                        }
                    }
                    checkJob = JobUtils.mapFromDtoToDbModelForUpdating(dto, checkJob);
                    if (checkJob.salary_min > checkJob.salary_max || checkJob.salary_max < checkJob.salary_min)//kiểm tra lại ràng buộc giữa salary max và salary min
                    {
                        return 3;
                    }
                    checkJob.status = 1;
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

        public List<ReturnPendingJobDTO> getAllPendingJobs()
        {
            List<ReturnPendingJobDTO> result = new List<ReturnPendingJobDTO>();
            using (context)
            {
                result = context.jobs.AsEnumerable().Where(s => s.status == 1).Select(s =>
                    JobUtils.mapFromDbContextToPendingDTO(s)
                ).ToList<ReturnPendingJobDTO>();

                //xử lý thêm category vào dto trả về cho frontend
                foreach (ReturnPendingJobDTO element in result)
                {
                    var recruiter = context.recruiters.Find(element.recruiterId);
                    var price = context.active_days_price.Where(s => s.active_days == element.activeDays).Select(s => s.price).FirstOrDefault();
                    element.price = price;
                    element.recruiterUsername = recruiter.username;
                    if (recruiter.companies.Count != 0)
                    {
                        element.companyName = recruiter.companies.FirstOrDefault().name;
                    }
                    else
                        element.companyName = "Không có";
                    element.categories = new List<Category>();
                    foreach (job_has_category relationship in element.relationship)
                    {
                        Category category = context.categories.AsEnumerable().Where(s => s.id == relationship.category_id).Select(s => new Category()
                        {
                            id = s.id,
                            value = s.value
                        }).FirstOrDefault<Category>();
                        element.categories.Add(category);
                    }
                }

            }
            return result;
        }

        public bool denyAJob(int jobId, string message, int staffId)
        {
            using (context)
            {
                var staff = context.managers.Find(staffId);
                if (staff == null || staff.is_banned == true)
                {
                    return false;
                }
                else
                {
                    var job = context.jobs.Find(jobId);
                    if (job == null || job.status == 2 || job.status == 3)
                    {
                        return false;
                    }
                    else
                    {
                        try
                        {
                            using (var dbTransaction = context.Database.BeginTransaction())
                            {
                                manager_deny_job relationship = new manager_deny_job();
                                relationship.deny_message = message;
                                relationship.job_id = jobId;
                                relationship.manager_id = staffId;
                                relationship.recruiter_id = job.recruiter_id;
                                context.manager_deny_job.Add(relationship);
                                context.SaveChanges();
                                job.status = 3;
                                job.manager_id = staffId;
                                context.SaveChanges();
                                dbTransaction.Commit();
                            }
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

        public bool approveAJob(int jobId, int staffId)
        {
            using (context)
            {
                var staff = context.managers.Find(staffId);
                if (staff == null || staff.is_banned == true)
                {
                    return false;
                }
                var job = context.jobs.Find(jobId);
                if (job == null) return false;//ko tìm thấy job
                else
                {
                    if (job.status == 2) return true;//đã duyệt rồi rồi,duyệt nữa chi ? 
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

        //Dư,ko cần thiết
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
                        Category category = context.categories.AsEnumerable().Where(s => s.id == relationship.category_id).Select(s => new Category()
                        {
                            id = s.id,
                            value = s.value
                        }).FirstOrDefault<Category>();
                        element.categories.Add(category);
                    }
                }

            }
            return result;
        }

        public int applyAJob(int jobId, int studentId, int cvId)
        {
            using (context)
            {
                var job = context.jobs.Find(jobId);
                var student = context.students.Find(studentId);
                if (student == null || !student.profile_status) return 3;
                IList<int> listCvsOfThisStudent = student.cvs.Select(s => s.id).ToList<int>();
                if (!listCvsOfThisStudent.Contains(cvId))
                {
                    return 7;
                }
                if (job == null) return 2;
                else
                {
                    if (job.status == 1) return 5;
                    if (DateTimeUtils.isOverAfterDays(job.create_date, job.active_days)) return 4;
                }
                var checkApplied = context.student_apply_job.
                    Where(s => s.cv_id == cvId && s.student_id == studentId && s.job_id == jobId).FirstOrDefault<student_apply_job>();
                if (checkApplied != null)
                {
                    return 8;
                }

                try
                {
                    student_apply_job relationship = new student_apply_job();
                    relationship.create_date = DateTime.Now;
                    relationship.job_id = jobId;
                    relationship.student_id = studentId;
                    relationship.cv_id = cvId;
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
                list = list.Cast<Job>().Where(s => StringUtils.convertToUnSign3(s.name).ToLower().Contains(searchDTO.keyword.ToLower())).ToList<Job>();
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
                result = context.jobs.AsEnumerable().Where(s => s.status == 2 && !DateTimeUtils.isOverAfterDays(s.create_date, s.active_days)).Select(s => JobUtils.mapFromDbContext(s)).ToList<Job>();
                foreach (Job element in result)
                {
                    element.categories = new List<Category>();
                    foreach (job_has_category relationship in element.relationship)
                    {
                        Category category = context.categories.AsEnumerable().Where(s => s.id == relationship.category_id).Select(s => new Category()
                        {
                            id = s.id,
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
                !DateTimeUtils.isOverAfterDays(s.create_date, s.active_days) && !JobUtils.hasThisStudentApplied(studentId, s)).Select(s => JobUtils.mapFromDbContext(s)).ToList<Job>();
            }
            foreach (Job element in listSearchFromDB)
            {
                if (element.weightCompareToLastAppliedJob(studentLastAppliedJobString) >= 2)
                {
                    result.Add(element);
                }
            }
            if (result.Count > 4)
            {
                result.RemoveRange(4, result.Count - 4);
            }
            return result;
        }

        public IList<Job> getPostedJobByRecruiterId(int recruiterId)
        {
            IList<Job> listResult = new List<Job>();
            using (context)
            {
                listResult = context.jobs.AsEnumerable().Where(s => s.recruiter_id == recruiterId)
                    .OrderByDescending(s => s.create_date)
                    .Select(s => JobUtils.mapFromDbContext(s)).ToList<Job>();
                foreach (Job element in listResult)
                {
                    if (DateTimeUtils.isOverAfterDays(element.createDate2, element.activeDays))
                    {
                        element.isOver = true;
                    }
                    element.endDate = element.createDate2.AddDays(element.activeDays).ToString("dd/MM/yyyy");
                    element.categories = new List<Category>();
                    foreach (job_has_category relationship in element.relationship)
                    {
                        Category category = context.categories.AsEnumerable().Where(s => s.id == relationship.category_id).Select(s => new Category()
                        {
                            id = s.id,
                            value = s.value
                        }).FirstOrDefault<Category>();
                        element.categories.Add(category);
                    }
                }
            }
            return listResult;
        }

        public IList<AppliedJobDTO> getAppliedJobByStudentId(int studentId)
        {
            IList<AppliedJobDTO> listResult = new List<AppliedJobDTO>();
            using (context)
            {
                IList<student_apply_job> listRelationship = context.student_apply_job.Where(s => s.student_id == studentId).ToList<student_apply_job>();
                foreach (student_apply_job element in listRelationship)
                {
                    AppliedJobDTO dto = new AppliedJobDTO();
                    dto.id = element.job_id;
                    var job = context.jobs.Find(dto.id);
                    if (job != null)
                    {
                        dto.name = job.name;
                        dto.jobEndDate = job.create_date.AddDays(job.active_days).ToString("dd/MM/yyyy");
                        dto.appliedDate = element.create_date.ToString("dd/MM/yyyy");
                        listResult.Add(dto);
                    }
                }
            }
            return listResult;
        }

        public IList<ActiveDaysAndPrice> getAllActiveDaysAndPrice()
        {
            List<ActiveDaysAndPrice> result = new List<ActiveDaysAndPrice>();
            using (context)
            {
                result = context.active_days_price.AsEnumerable().Select(s => new ActiveDaysAndPrice() { id = s.id, activeDays = s.active_days, price = s.price }).ToList<ActiveDaysAndPrice>();
            }
            return result;
        }

        public IList<Category> getAllCategories()
        {
            IList<Category> listResult = new List<Category>();
            using (context)
            {
                listResult = context.categories.Select(s => new Category()
                {
                    id = s.id,
                    value = s.value
                }).ToList<Category>();
            }
            return listResult;
        }

        public int getTotalPages()
        {
            int result = 0;
            using (context)
            {
                int count = context.jobs.AsEnumerable().Where(s => s.status == 2 && !DateTimeUtils.isOverAfterDays(s.create_date, s.active_days)).ToList<job>().Count;
                result = (int)Math.Ceiling((double)count / 5);
            }
            return result;
        }

        public IList<Job> getAllWithPaging(int page)
        {
            if (page == 0)
                page = 1;
            IList<Job> listResult = new List<Job>();
            using (context)
            {
                listResult = context.jobs.AsEnumerable().Where(s => s.status == 2 && !DateTimeUtils.isOverAfterDays(s.create_date, s.active_days))
                   .Select(s => JobUtils.mapFromDbContext(s))
                   .OrderByDescending(s => s.createDate2)
                   .Skip(5 * (page - 1))
                   .Take(5)
                   .ToList<Job>();
                foreach (Job element in listResult)
                {
                    element.categories = new List<Category>();
                    element.endDate = element.createDate2.AddDays(element.activeDays).ToString("dd/MM/yyyy");
                    foreach (job_has_category relationship in element.relationship)
                    {
                        Category category = context.categories.AsEnumerable().Where(s => s.id == relationship.category_id).Select(s => new Category()
                        {
                            id = s.id,
                            value = s.value
                        }).FirstOrDefault<Category>();
                        element.categories.Add(category);
                    }
                }
            }
            return listResult;
        }

        public IList<DeniedJobDTO> getAllDeniedJobs(int recruiterId)
        {
            IList<DeniedJobDTO> result = new List<DeniedJobDTO>();
            using (context)
            {
                result = context.jobs.AsEnumerable().Where(s => s.status == 3 && s.recruiter_id == recruiterId).Select(s => JobUtils.mapToDeniedJob(s)).ToList<DeniedJobDTO>();
                foreach (DeniedJobDTO element in result)
                {
                    string message = context.manager_deny_job.Where(s => s.job_id == element.id).Select(s => s.deny_message).FirstOrDefault();
                    element.denyMessage = message;
                }
            }
            return result;
        }

        public IList<Banner> getAllBanners()
        {
            IList<Banner> result = new List<Banner>();
            using (context)
            {
                result = context.banners.Select(s => new Banner() { id = s.id, url = s.url, imgUrl = s.image_url }).ToList<Banner>();
            }
            return result;
        }

        public IList<SimilarJobDTO> getSimilarJobs(int jobId)
        {
            IList<SimilarJobDTO> result = new List<SimilarJobDTO>();
            using (context)
            {
                var job = context.jobs.Find(jobId);
                if (job != null)
                {
                    IList<job_has_category> categories = job.job_has_category.ToList<job_has_category>();
                    int categoryId = categories.Select(s => s.category_id).FirstOrDefault();
                    result = context.jobs.AsEnumerable()
                        .Where(s => s.job_has_category.Select(t => t.category_id).FirstOrDefault() == categoryId && s.id != jobId && !DateTimeUtils.isOverAfterDays(s.create_date, s.active_days))
                        .Select(s => new SimilarJobDTO() { id = s.id, name = s.name, salaryMax = s.salary_max, salaryMin = s.salary_min })
                        .Take(2)
                        .ToList<SimilarJobDTO>();
                }
            }
            return result;
        }
    }
}