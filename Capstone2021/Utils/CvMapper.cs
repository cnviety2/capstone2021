using Capstone2021.DTO;
using System;
using System.Web.WebPages;

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
            result.dob = model.dob.Value.ToString("dd/MM/yyyy");
            result.createDate = model.create_date.Value.ToString("dd/MM/yyyy");
            result.avatar = model.avatar;
            result.desiredSalaryMinimum = (int)model.desired_salary_minimum;
            result.skill = model.skill;
            result.cvName = model.cv_name;
            return result;
        }
        ///<summary>
        ///Map từ Cv sang cv để lưu xuống db
        ///</summary>
        public static cv mapToDatabaseModel(CreateCvDTO dto)
        {
            cv model = new cv();
            model.cv_name = dto.cvName.Trim();
            model.avatar = "";
            model.name = dto.name.Trim();
            model.sex = dto.sex;
            model.working_form = dto.workingForm;
            model.school = dto.school != null ? dto.school.Trim() : "";
            model.is_subscribed = false;
            model.foreign_language = dto.foreignLanguage != null ? dto.foreignLanguage.Trim() : "";
            model.experience = dto.experience != null ? dto.experience.Trim() : "";
            model.dob = DateTimeUtils.parse(dto.dob);
            model.create_date = DateTime.Now;
            model.skill = dto.skill != null ? dto.skill.Trim() : "";
            model.desired_salary_minimum = dto.desiredSalaryMinimum;
            model.is_public = false;
            return model;
        }
        /// <summary>
        /// Hàm map từ UpadteCvDTO sang model trong db để update,sẽ kiểm tra trong dto field nào khác null thì mới map sang model
        /// </summary>
        /// <param name="source"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static cv mapFromDtoToDbModelForUpdating(UpdateCvDTO dto, cv model)
        {
            if (dto.name != null || !dto.name.IsEmpty())
            {
                model.name = dto.name;
            }
            if (dto.school != null || !dto.school.IsEmpty())
            {
                model.school = dto.school;
            }
            if (dto.experience != null || !dto.experience.IsEmpty())
            {
                model.experience = dto.experience;
            }
            if (dto.foreignLanguage != null || !dto.foreignLanguage.IsEmpty())
            {
                model.foreign_language = dto.foreignLanguage;
            }
            if (dto.sex.HasValue && dto.sex != model.sex)
            {
                model.sex = dto.sex;
            }
            if (dto.desiredSalaryMinimum.HasValue && dto.desiredSalaryMinimum != model.desired_salary_minimum && dto.desiredSalaryMinimum != 0)
            {
                model.desired_salary_minimum = dto.desiredSalaryMinimum;
            }
            if (dto.workingForm.HasValue && dto.workingForm != model.working_form && dto.workingForm != 0)
            {
                model.working_form = dto.workingForm.Value;
            }
            if (dto.skill != null || !dto.skill.IsEmpty())
            {
                model.skill = dto.skill;
            }
            if (dto.dob != null || !dto.dob.IsEmpty())
            {
                model.dob = DateTimeUtils.parse(dto.dob);
            }
            if (dto.cvName != null || !dto.cvName.IsEmpty())
            {
                model.cv_name = dto.cvName.Trim();
            }
            return model;
        }
    }
}