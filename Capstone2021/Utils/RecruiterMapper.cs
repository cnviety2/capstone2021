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
            result.avatar = recruiter.avatar;
            result.create_date = recruiter.createDate;
            result.gmail = recruiter.gmail;
            result.password = recruiter.password;
            result.phone = recruiter.phone;
            result.role = recruiter.role;
            result.username = recruiter.username;
            result.full_name = recruiter.fullName;

            return result;
        }

        public static Recruiter mapFromDto(CreateRecruiterDTO dto)
        {
            Recruiter result = new Recruiter();
            result.avatar = dto.avatar;
            result.createDate = DateTime.Now;
            result.gmail = dto.gmail;
            result.password = dto.password;
            result.phone = dto.phone;
            result.role = "ROLE_RECRUITER";
            result.username = dto.username;
            result.fullName = dto.fullname;

            return result;
        }
    }
}