using Capstone2021.DTO;
using Capstone2021.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Capstone2021.Controllers
{
    [RoutePrefix("job")]
    public class JobController : ApiController
    {
        private JobService jobService;

        public JobController()
        {
            jobService = new JobServiceImpl();
        }

        [HttpGet]
        [Route("{id:int:min(0)}")]
        public IHttpActionResult getAJob([FromUri] int id)
        {
            ResponseDTO response = new ResponseDTO();
            Job manager = jobService.get(id);
            if (manager == null)
            {
                return NotFound();
            }
            else
            {
                response.message = "OK";
            }
            response.data = manager;
            return Ok(response);
        }
    }
}