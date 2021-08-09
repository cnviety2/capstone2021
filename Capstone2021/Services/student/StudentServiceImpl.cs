using Capstone2021.DTO;
using Capstone2021.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone2021.Services.Student
{
    public class StudentServiceImpl : StudentService
    {
        private static Logger logger;
        private DbEntities context;

        public StudentServiceImpl()
        {
            logger = LogManager.GetCurrentClassLogger();
            context = new DbEntities();
        }
        public bool create(DTO.Student obj)
        {
            var std = context.students.Where(s => s.gmail.Equals(obj.gmail)).Select(s => new DTO.Student()
            {
                gmail = s.gmail,
                googleId = s.google_id
            }).FirstOrDefault<DTO.Student>();
            if (std != null)
            {
                if (std.googleId.Equals(obj.googleId)) return true;
                else return false;
            }
            else
            {
                try
                {
                    student saveObj = StudentMapper.mapToDbModel(obj);
                    context.students.Add(saveObj);
                    context.SaveChanges();
                    obj = StudentMapper.mapToDto(saveObj);
                    return true; ;
                }
                catch (Exception e)
                {
                    logger.Info("Exception " + e.Message + "in StudentServiceImpl");
                    return false;
                }

            }
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public DTO.Student get(int id)
        {
            DTO.Student result = null;
            using (context)
            {
                //context.managers sẽ lấy ra DataSet của table manager ở phía dưới db
                result = context.students.AsEnumerable().Where(s => s.id == id).Select(s => new DTO.Student()
                {
                    id = s.id,
                    gmail = s.gmail,
                    isBanned = s.is_banned.Value,
                    createDate = s.create_date,
                    profileStatus = s.profile_status,
                    avatar = s.avatar
                    //googleId = s.google_id
                }).FirstOrDefault<DTO.Student>();
            }
            return result;
        }

        public IList<DTO.Student> getAll()
        {
            throw new NotImplementedException();
        }

        public IList<DTO.ReturnAppliedStudentDTO> getAppliedStudentsOfThisJob(int jobId)
        {
            IList<ReturnAppliedStudentDTO> result = new List<ReturnAppliedStudentDTO>();
            using (context)
            {
                var job = context.jobs.Find(jobId);
                if (job == null)
                {
                    return null;
                }
                else
                {
                    IList<StudentIdAndCvIdDTO> listStudentdApplied = context.student_apply_job.AsEnumerable().Where(s => s.job_id == jobId).Select(s => new StudentIdAndCvIdDTO()
                    {
                        cvId = s.cv_id,
                        studentId = s.student_id,
                        appliedDate = s.create_date.ToString("dd/MM/yyyy")
                    }).ToList<StudentIdAndCvIdDTO>();
                    if (listStudentdApplied.Count == 0)
                    {
                        return result;
                    }
                    foreach (StudentIdAndCvIdDTO dto in listStudentdApplied)
                    {
                        var student = context.students.Find(dto.studentId);
                        ReturnCvDTO cv = context.cvs.Where(s => s.id == dto.cvId).Select(s => new ReturnCvDTO()
                        {
                            avatar = s.avatar,
                            desiredSalaryMinimum = s.desired_salary_minimum.Value,
                            dob = s.dob.Value.ToString(),
                            experience = s.experience,
                            foreignLanguage = s.foreign_language,
                            name = s.name,
                            school = s.school,
                            sex = s.sex.Value,
                            workingForm = s.working_form.Value,
                            skill = s.skill,
                            cvName = s.cv_name,
                            phone = s.phone
                        }).FirstOrDefault<ReturnCvDTO>();
                        if (cv == null)
                        {
                            return null;
                        }
                        ReturnAppliedStudentDTO returnStudent = new ReturnAppliedStudentDTO() { gmail = student.gmail, id = student.id };
                        returnStudent.cv = cv;
                        result.Add(returnStudent);
                    }
                }
                return result;
            }
        }

        public string getLastAppliedJobString(int studentId)
        {
            using (context)
            {
                var student = context.students.Find(studentId);
                if (student == null)
                {
                    return null;
                }
                else
                {
                    return student.last_applied_job_string;
                }
            }
        }

        public IList<ReturnSavedJobDTO> getSavedJobs(int studentId)
        {
            IList<ReturnSavedJobDTO> result = new List<ReturnSavedJobDTO>();
            using (context)
            {
                IList<int> listSaveJobId = context.student_save_job.AsEnumerable().Where(s => s.student_id == studentId).Select(s => s.job_id)
                    .ToList<int>();
                if (listSaveJobId.Count == 0)
                {
                    return result;
                }
                else
                {
                    foreach (int jobId in listSaveJobId)
                    {
                        var job = context.jobs.Find(jobId);
                        ReturnSavedJobDTO saveJobDTO = new ReturnSavedJobDTO() { id = job.id, name = job.name };
                        result.Add(saveJobDTO);
                    }
                }
            }
            return result;
        }

        public ReturnStudentDTO getSelfInfo(int studentId)
        {
            using (context)
            {
                var student = context.students.Find(studentId);
                if (student == null)
                {
                    return null;
                }
                else
                {
                    ReturnStudentDTO result = new ReturnStudentDTO();
                    result.id = student.id;
                    result.googleId = student.google_id;
                    result.gmail = student.gmail;
                    result.phone = student.phone;
                    result.profileStatus = student.profile_status;
                    result.avatar = student.avatar;
                    result.createDate = student.create_date.ToString("dd/MM/yyyy");
                    if (student.cvs.Count != 0)
                    {
                        result.listCv = new List<ReturnListCvDTO>();
                        foreach (cv element in student.cvs)
                        {
                            ReturnListCvDTO cv = new ReturnListCvDTO();
                            cv.id = element.id;
                            cv.createDate = element.create_date.Value.ToString("dd/MM/yyyy");
                            cv.cvName = element.cv_name;
                            cv.desiredSalary = element.desired_salary_minimum.Value;
                            cv.workingForm = element.working_form.Value;
                            result.listCv.Add(cv);
                        }
                    }
                    return result;
                }
            }
        }

        public DTO.Student login(DTO.Student obj)
        {
            using (context)
            {
                DTO.Student std = context.students.AsEnumerable().Where(s => s.gmail.Equals(obj.gmail)).Select(s => new DTO.Student()
                {
                    id = s.id,
                    gmail = s.gmail,
                    isBanned = s.is_banned.Value,
                    avatar = s.avatar,
                    googleId = s.google_id,
                    profileStatus = s.profile_status
                }
                ).FirstOrDefault<DTO.Student>();

                if (std == null)
                {
                    try
                    {
                        student saveObj = StudentMapper.mapToDbModel(obj);
                        context.students.Add(saveObj);
                        context.SaveChanges();
                        obj = StudentMapper.mapToDto(saveObj);
                        return obj;
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in StudentServiceImpl");
                        return null;
                    }
                }

                if (std != null)
                {
                    if (std.googleId.Equals(obj.googleId)) return std;
                    else return null;
                }
            }
            return null;
        }

        public bool remove(int id)
        {
            throw new NotImplementedException();
        }

        public int removeSavedJob(int jobId, int studentId)
        {
            using (context)
            {
                var relationship = context.student_save_job.SingleOrDefault(s => s.student_id == studentId && s.job_id == jobId);
                if (relationship == null)
                    return 2;
                else
                {
                    try
                    {
                        context.student_save_job.Remove(relationship);
                        context.SaveChanges();
                        return 1;
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in StudentServiceImpl");
                        return 3;
                    }
                }
            }
        }

        public int saveJob(int jobId, int studentId)
        {
            using (context)
            {
                var job = context.jobs.Find(jobId);
                if (job == null)
                {
                    return 3;
                }
                IList<student_save_job> check = context.student_save_job.AsEnumerable().Where(s => s.student_id == studentId).Select(s => new student_save_job()
                {
                    job_id = s.job_id
                }).ToList<student_save_job>();
                foreach (student_save_job element in check)
                {
                    if (element.job_id == jobId)
                        return 2;
                }
                {
                    try
                    {
                        student_save_job relatetionship = new student_save_job();
                        relatetionship.job_id = jobId;
                        relatetionship.student_id = studentId;
                        relatetionship.createDate = DateTime.Now;
                        context.student_save_job.Add(relatetionship);
                        context.SaveChanges();
                        return 1;
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in StudentServiceImpl");
                        return 4;
                    }
                }
            }
        }

        public bool softRemove(int id)
        {
            throw new NotImplementedException();
        }

        public string updateImage(string imageUrl, int id)
        {
            string url = "";
            var checkStudent = context.students.Find(id);
            if (checkStudent == null)
                return url;
            using (context)
            {
                try
                {
                    checkStudent.avatar = "https://capstone2021-fpt.s3.ap-southeast-1.amazonaws.com/" + imageUrl;
                    context.SaveChanges();
                    url = checkStudent.avatar;
                    return url;

                }
                catch (Exception e)
                {
                    logger.Info("Exception " + e.Message + "in StudentServiceImpl");
                    return url;
                }
            }
        }
    }
}