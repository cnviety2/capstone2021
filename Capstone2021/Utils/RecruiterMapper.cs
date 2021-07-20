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

            return result;
        }
        /// <summary>
        /// Method để map từ dto Recruiter sang model của entity framework để lưu xuống databse
        /// </summary>
        public static recruiter mapToDatabaseModel(CreateRecruiterDTO dto)
        {
            recruiter model = new recruiter();
            model.avatar = dto.avatar;
            model.create_date = DateTime.Now;
            model.gmail = dto.gmail;
            model.password = dto.password;
            model.phone = dto.phone;
            model.role = dto.role;
            model.username = dto.username;

            return model;
        }
    }
}