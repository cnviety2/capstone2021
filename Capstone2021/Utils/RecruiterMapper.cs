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
            result.avatar = recruiter.avatar.Trim();
            result.create_date = recruiter.createDate;
            result.gmail = recruiter.gmail.Trim();
            result.password = recruiter.password.Trim();
            result.phone = recruiter.phone.Trim();
            result.role = recruiter.role.Trim();
            result.username = recruiter.username.Trim();
            result.full_name = recruiter.fullName.Trim();
            result.sex = recruiter.sex;
            return result;
        }

        public static Recruiter mapFromDto(CreateRecruiterDTO dto)
        {
            Recruiter result = new Recruiter();
            result.createDate = DateTime.Now;
            result.gmail = dto.gmail.Trim();
            result.password = dto.password.Trim();
            result.phone = dto.phone.Trim();
            result.role = "ROLE_RECRUITER";
            result.username = dto.username.Trim();
            result.fullName = dto.fullname.Trim();
            result.sex = dto.sex;
            return result;
        }
    }
}