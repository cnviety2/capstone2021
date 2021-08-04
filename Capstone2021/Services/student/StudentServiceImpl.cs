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
                    IList<int> listStudentIdApplied = context.student_apply_job.AsEnumerable().Where(s => s.job_id == jobId).Select(s => s.student_id).ToList<int>();
                    if (listStudentIdApplied == null || listStudentIdApplied.Count == 0)
                    {
                        return result;
                    }
                    foreach (int studentId in listStudentIdApplied)
                    {
                        var student = context.students.Find(studentId);
                        ReturnCvDTO cv = context.cvs.Where(s => s.student_id == student.id).Select(s => new ReturnCvDTO()
                        {
                            avatar = s.avatar,
                            desiredSalaryMinimum = s.desired_salary_minimum.Value,
                            dob = s.dob.Value,
                            experience = s.experience,
                            foreignLanguage = s.foreign_language,
                            name = s.name,
                            school = s.school,
                            sex = s.sex.Value,
                            workingForm = s.working_form.Value
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

        public bool softRemove(int id)
        {
            throw new NotImplementedException();
        }

        public bool updateImage(string imageUrl, int id)
        {
            var checkStudent = context.students.Find(id);
            if (checkStudent == null)
                return false;
            using (context)
            {
                try
                {
                    checkStudent.avatar = "https://capstone2021-fpt.s3.ap-southeast-1.amazonaws.com/" + imageUrl;
                    context.SaveChanges();
                    return true;

                }
                catch (Exception e)
                {
                    logger.Info("Exception " + e.Message + "in StudentServiceImpl");
                    return false;
                }
            }
        }
    }
}