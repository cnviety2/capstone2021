using Capstone2021.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone2021.Controllers
{
    [RoutePrefix("student")]
    [Authorize(Roles = "ROLE_STUDENT")]//chỉ có những user có role là student chứa trong HttpContext là được sử dụng những api này
    public class StudentController : Controller
    {
        private readonly StudentService _studentService;
        public StudentController()
        {
            _studentService = new StudentServiceImpl();
        }
        
    }
}