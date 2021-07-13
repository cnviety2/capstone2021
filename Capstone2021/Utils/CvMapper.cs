using Capstone2021.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
            result.name = model.name;
            result.sex = (bool)model.sex;
            result.workingForm = (int)model.working_form;
            result.school = model.school;
            result.isSubscribed = (bool)model.is_subscribed;
            result.foreignLanguage = model.foreign_language;
            result.experience = model.experience;
            result.dob = (DateTime)model.dob;
            result.createDate = (DateTime)model.create_date;
            result.avatar = model.avatar;
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
            result.name = obj.name;
            result.sex = obj.sex;
            result.working_form = obj.workingForm;
            result.school = obj.school;
            result.is_subscribed = obj.isSubscribed;
            result.foreign_language = obj.foreignLanguage;
            result.experience = obj.experience;
            result.dob = obj.dob;
            result.create_date = obj.createDate;
            result.avatar = obj.avatar;
            result.desired_salary_minimum = obj.desiredSalaryMinimum;
            return result;
        }
    }
}