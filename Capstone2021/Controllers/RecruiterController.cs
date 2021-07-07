using Capstone2021.DTO;
using Capstone2021.Services;
using System.Web.Helpers;
using System.Web.Http;

namespace Capstone2021.Controllers
{
    [RoutePrefix("recruiter")]
    [Authorize(Roles = "ROLE_RECRUITER")]//chỉ có những user có role là recruiter chứa trong HttpContext là được sử dụng những api này
    public class RecruiterController : ApiController
    {
        private readonly RecruiterService _recruiterService;
        public RecruiterController()
        {
            _recruiterService = new RecruiterServiceImpl();
        }
        /*
                [Route("{id}")]
                [HttpGet]
                public IHttpActionResult getAllJob([FromUri] int id)
                {
                    ResponseDTO response = new ResponseDTO();
                    IList<Job> list = (IList<Job>)_recruiterService.getAllJob(id);
                    if (list.Count == 0)
                    {
                        response.message = "Bạn không có tin tuyển dụng công việc";
                        return Ok(response);
                    }
                    response.message = "OK";
                    response.data = list;
                    return Ok(response);
                }*/

        [Route("create")]
        [HttpPost]
        [AllowAnonymous]
        public IHttpActionResult create([FromBody] Recruiter recruiter)
        {
            ResponseDTO response = new ResponseDTO();
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            recruiter.password = Crypto.HashPassword(recruiter.password.Trim());
            bool saveState = _recruiterService.create(recruiter);
            if (saveState)
            {
                response.message = "OK";
            }
            else
            {
                response.message = "Error occured";
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult test()
        {
            return Ok();
        }
    }
}