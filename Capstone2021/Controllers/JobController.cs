using Capstone2021.DTO;
using Capstone2021.Services;
using Capstone2021.Utils;
using System.Collections.Generic;
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
        [Route("")]
        public IHttpActionResult getAllJob()
        {
            ResponseDTO response = new ResponseDTO();
            IList<Job> list = jobService.getAll();
            if (list.Count == 0)
            {
                response.message = "No data";
                return Ok(response);
            }
            response.message = "OK";
            response.data = list;
            return Ok(response);
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
            bool createState = jobService.create(dto, id);
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

        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "ROLE_RECRUITER")]
        public IHttpActionResult updateAJob([FromBody] UpdateJobDTO dto)
        {
            bool flag = false;
            if (dto.isEmpty())
            {
                ModelState.AddModelError("dto", "Data to update is empty");
                flag = true;
            }
            if (dto.workingForm.HasValue && (dto.workingForm < 1 || dto.workingForm > 3))
            {
                ModelState.AddModelError("dto.workingForm", "`Part-time:1,Full-time:2,Both:3");
                flag = true;
            }
            if (dto.location.HasValue && (dto.location < 1 || dto.location > 24))
            {
                ModelState.AddModelError("dto.location", "District 1 to 12 : 1 -> 12,13 : bình tân,14 : bình thạnh,15 : gò vấp,16 : phú nhuận,17 : tân bình,18 : " +
            "tân phú,19 : thủ đức,20 : bình chánh,21 : cần giờ,22 : củ chi,23 : hóc môn,24 : nhà bè");
                flag = true;
            }
            if (dto.sex.HasValue && (dto.sex < 1 || dto.sex > 3))
            {
                ModelState.AddModelError("dto.sex", "1 is male,2 is female,3 is both");
                flag = true;
            }
            if (dto.quantity.HasValue && dto.quantity < 1)
            {
                ModelState.AddModelError("dto.quantity", "Minimum value is 1");
                flag = true;
            }
            if (dto.salaryMin.HasValue && dto.salaryMin < 1)
            {
                ModelState.AddModelError("dto.salaryMin", "Minimum value is 1");
                flag = true;
            }
            if (dto.salaryMax.HasValue && dto.salaryMax < 1)
            {
                ModelState.AddModelError("dto.salaryMax", "Minimum value is 1");
                flag = true;
            }
            if ((dto.salaryMax.HasValue && dto.salaryMin.HasValue) && dto.salaryMin >= dto.salaryMax)
            {
                ModelState.AddModelError("dto.salaryMin", "salaryMin can't >= salaryMax");
                flag = true;
            }
            if (flag || dto == null)
            {
                return BadRequest(ModelState);
            }
            ResponseDTO response = new ResponseDTO();
            int updateState = jobService.update(dto, dto.id);
            switch (updateState)
            {
                case -1:
                    response.message = "Doesn't exist";
                    break;
                case 0:
                    response.message = "OK,status changed to pending";
                    break;
                case 1:
                    response.message = "Error occured";
                    break;
                case 2:
                    response.message = "This job was updated,in pending state";
                    break;
                case 3:
                    ModelState.AddModelError("dto.salary", "salaryMin can't >= salaryMax or salaryMax <= salaryMin");
                    return BadRequest(ModelState);
            }
            return Ok(response);

        }
    }
}