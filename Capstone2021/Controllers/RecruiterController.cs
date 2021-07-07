using Capstone2021.DTO;
using Capstone2021.Service;
using Capstone2021.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

namespace Capstone2021.Controllers
{
    [RoutePrefix("admin")]
    [Authorize(Roles = "ROLE_RECRUITER")]//chỉ có những user có role là recruiter chứa trong HttpContext là được sử dụng những api này
    public class RecruiterController : ApiController
    {
        private readonly RecruiterService _recruiterService;
        public RecruiterController(RecruiterService recruiterService)
        {
            _recruiterService = new RecruiterServiceImpl();
        }
        [Route("{id}")]
        [HttpGet]
        public IHttpActionResult getAllJob([FromUri] int id)
        {
            ResponseDTO response = new ResponseDTO();
            IList<Job> list = (IList<Job>)_recruiterService.getAllJob(id);
            if(list.Count == 0)
            {
                response.message = "Bạn không có tin tuyển dụng công việc";
                return Ok(response);
            }
            response.message = "OK";
            response.data = list;
            return Ok(response);
        }
    }
}