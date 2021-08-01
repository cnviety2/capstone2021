using Capstone2021.DTO;
using System;

namespace Capstone2021.Utils
{
    public class CvMapper
    {
        /// <summary>
        /// Method để map từ model của Db sang dto Cv,dùng trong service của cv khi get 1 cv từ Db
        /// </summary>
        public static Cv getFromDbContext(cv model)
        {
            Cv result = new Cv();
            result.studentId = model.student_id;
            result.name = model.name.Trim();
            result.sex = (bool)model.sex;
            result.workingForm = (int)model.working_form;
            result.school = model.school.Trim();
            result.isSubscribed = (bool)model.is_subscribed;
            result.foreignLanguage = model.foreign_language.Trim();
            result.experience = model.experience.Trim();
            result.dob = (DateTime)model.dob;
            result.createDate = (DateTime)model.create_date;
            result.avatar = model.avatar.Trim();
            result.desiredSalaryMinimum = (int)model.desired_salary_minimum;
            return result;
        }
        ///<summary>
        ///Map từ Cv sang cv để lưu xuống db
        ///</summary>
        public static cv map(Cv obj)
        {
            cv result = new cv();
            result.student_id = obj.studentId;
            result.name = obj.name.Trim();
            result.sex = obj.sex;
            result.working_form = obj.workingForm;
            result.school = obj.school.Trim();
            result.is_subscribed = false;
            result.foreign_language = obj.foreignLanguage.Trim();
            result.experience = obj.experience.Trim();
            result.dob = obj.dob;
            result.create_date = DateTime.Now;
            result.avatar = obj.avatar.Trim();
            result.desired_salary_minimum = obj.desiredSalaryMinimum;
            result.is_public = false;
            return result;
        }
    }
}