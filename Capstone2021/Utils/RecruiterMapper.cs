using Capstone2021.DTO;
using System;

namespace Capstone2021.Utils
{
    public class RecruiterMapper
    {
        ///<summary>Map từ Recruiter sang recruiter để lưu xuống db,ko có phần jobs</summary>
        public static recruiter map(Recruiter recruiter)
        {
            recruiter result = new recruiter();
            result.create_date = DateTime.Now;
            result.gmail = recruiter.gmail.Trim();
            result.password = recruiter.password.Trim();
            result.phone = recruiter.phone.Trim();
            result.role = "ROLE_RECRUITER";
            result.username = recruiter.username.Trim();
            result.first_name = recruiter.firstName.Trim();
            result.last_name = recruiter.lastName.Trim();
            result.sex = recruiter.sex;
            result.avatar = "";
            result.is_banned = false;
            result.status = 0;
            return result;
        }

        public static Recruiter mapFromDto(CreateRecruiterDTO dto)
        {
            Recruiter result = new Recruiter();
            result.createDate = DateTime.Now.ToString("dd/MM/yyyy");
            result.gmail = dto.gmail.Trim();
            result.password = dto.password.Trim();
            result.phone = dto.phone.Trim();
            result.role = "ROLE_RECRUITER";
            result.username = dto.username.Trim();
            result.firstName = dto.firstname.Trim();
            result.lastName = dto.lastName.Trim();
            result.sex = dto.sex;
            return result;
        }

        public static recruiter mapFromDtoRegister(CreateRecruiterDTO dto)
        {
            recruiter model = new recruiter();
            model.username = dto.username.Trim();
            model.status = 0;
            model.role = "ROLE_RECRUITER";
            model.create_date = DateTime.Now;
            model.avatar = "";
            model.first_name = dto.firstname.Trim();
            model.last_name = dto.lastName.Trim();
            model.is_banned = false;
            model.password = dto.password;
            model.sex = dto.sex;
            model.phone = dto.phone.Trim();
            model.gmail = dto.gmail.Trim();
            return model;
        }
    }
}