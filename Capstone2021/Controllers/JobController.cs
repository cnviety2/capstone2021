using Capstone2021.DTO;
using Capstone2021.Services;
using Capstone2021.Utils;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace Capstone2021.Controllers
{
    [RoutePrefix("job")]
    [Authorize]
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

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "ROLE_RECRUITER")]
        public IHttpActionResult createAJob([FromBody] CreateJobDTO dto)
        {
            if (dto.salaryMin >= dto.salaryMax)
            {
                ModelState.AddModelError("dto.salaryMin", "salaryMin can't >= salaryMax");
                return BadRequest(ModelState);
            }
            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }
            ResponseDTO response = new ResponseDTO();
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;//claims đc lưu trong Request mà request có chứa token,từ token đó parse sang claims,
                                                                                              //có vài dữ liệu ko thể lấy đc từ HttpContext
            int id = HttpContextUtils.getUserID(claims);
            job model = JobMapper.mapToDatabaseModel(dto);
            bool createState = jobService.create(model, id);
            if (createState)
            {
                response.message = "OK";
            }
            else
            {
                return InternalServerError();
            }
            return Ok(response);

        }
    }
}