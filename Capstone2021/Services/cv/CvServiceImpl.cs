using Capstone2021.DTO;
using Capstone2021.Utils;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Capstone2021.Services
{
    /**
     * Class này hiện thực interface CvService
     */
    public class CvServiceImpl : CvService, IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private DbEntities context;
        public CvServiceImpl()
        {
            context = new DbEntities();//DbEntities là class đc Entity Framework tạo ra dùng để kết nối tới db,quản lý db đơn giản hơn
        }
        public bool create(Cv obj)
        {
            bool result = false;
            /*  int student_id = obj.studentId;
              cv saveObj = new cv();
              using (context)
              {
                  Cv checkCv = context.cvs.AsEnumerable().
                      Where(c => c.student_id.Equals(obj.studentId)).
                      Select(c => new Cv()
                      {
                          studentId = c.student_id,

                      }).FirstOrDefault<Cv>();
                  if (checkCv != null)
                  {
                      result = false;
                  }
                  else
                  {
                      try
                      {
                          saveObj = CvMapper.map(obj);
                          context.cvs.Add(saveObj);
                          context.SaveChanges();

                      }
                      catch (Exception e)
                      {
                          logger.Info("Exception " + e.Message + "in CvServiceImpl");
                          result = false;
                      }
                  }
              }*/
            return result;
        }

        public bool create(CreateCvDTO dto, int studentId)
        {
            using (context)
            {
                using (var contextTransaction = context.Database.BeginTransaction())
                {
                    var student = context.students.Find(studentId);
                    if (student != null)
                    {
                        if (student.profile_status == false)
                        {
                            try
                            {
                                cv model = CvMapper.mapToDatabaseModel(dto);
                                model.student_id = studentId;
                                context.cvs.Add(model);
                                context.SaveChanges();
                                student.profile_status = true;
                                context.SaveChanges();
                                contextTransaction.Commit();
                                return true;
                            }
                            catch (Exception e)
                            {
                                logger.Info("Exception " + e.InnerException.Message + "in CvServiceImpl");
                                contextTransaction.Rollback();
                                return false;
                            }
                        }
                    }
                    else
                    {
                        return false;
                    }
                    /*try
                    {
                        cv model = CvMapper.mapToDatabaseModel(dto);
                        model.student_id = studentId;
                        context.cvs.Add(model);
                        //đổi status profile student
                        int studentID = model.student_id;
                        var checkStudent = context.students.Find(studentID);
                        if(checkStudent == null)
                        {
                            result = false;//ko tim thay student
                        }
                        else
                        {
                            if(checkStudent.profile_status == true)
                            {
                                result = true;//da co cv
                            }
                            else
                            {
                                using (context)
                                {
                                    try
                                    {
                                        checkStudent.profile_status = true;
                                        Console.WriteLine(checkStudent);
                                        result = true;
                                        context.SaveChanges();
                                        contextTransaction.Commit();
                                    }
                                    catch (Exception e)
                                    {
                                        logger.Info("Exception " + e.InnerException.Message + "in CvServiceImpl");
                                        contextTransaction.Rollback();
                                        result = false;
                                    }
                                }
                            }
                        }
                    }
                    catch(Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in CvServiceImpl");
                        result = false;
                    }*/
                }
            }
            return false;
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public Cv get(int id)
        {
            Cv result = null;
            using (context)
            {
                //context.cvs sẽ lấy ra DataSet của table cv ở phía dưới db
                result = context.cvs.AsEnumerable()
                    .Where(c => c.student_id == id)
                    .Select(c => CvMapper.getFromDbContext(c))
                    .FirstOrDefault<Cv>();
            }
            return result;
        }

        public IList<Cv> getAll()
        {
            throw new NotImplementedException();
        }

        public bool remove(int id)
        {
            throw new NotImplementedException();
        }

        public bool softRemove(int id)
        {
            throw new NotImplementedException();
        }

        public bool update(UpdateCvDTO dto, int id)
        {
            bool result = false;
            using (context)
            {
                var cv = context.cvs
                    .SingleOrDefault(c => c.student_id.Equals(id));
                if (cv == null)
                {
                    return result;
                }
                else
                {
                    try
                    {
                        cv = CvMapper.mapFromDtoToDbModelForUpdating(dto, cv);
                        context.SaveChanges();
                        result = true;
                    }
                    catch (Exception e)
                    {
                        logger.Info("Exception " + e.Message + "in CvServiceImpl");
                        result = false;
                    }
                }
            }
            return result;
        }

        public bool updateImage(string imageUrl, int id)
        {
            var checkCv = context.cvs.Find(id);
            if (checkCv == null)
                return false;
            using (context)
            {
                try
                {
                    checkCv.avatar = "https://capstone2021-fpt.s3.ap-southeast-1.amazonaws.com/" + imageUrl;
                    context.SaveChanges();
                    return true;

                }
                catch (Exception e)
                {
                    logger.Info("Exception " + e.Message + "in CvServiceImpl");
                    return false;
                }
            }
        }
    }
}