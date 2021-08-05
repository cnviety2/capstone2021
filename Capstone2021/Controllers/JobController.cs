using Capstone2021.DTO;
using Capstone2021.Services;
using Capstone2021.Services.Student;
using Capstone2021.Utils;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.WebPages;

namespace Capstone2021.Controllers
{
    [RoutePrefix("job")]
    [Authorize]
    public class JobController : ApiController
    {
        private JobService jobService;
        private StudentService studentService;

        public JobController()
        {
            jobService = new JobServiceImpl();
            studentService = new StudentServiceImpl();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("categories")]
        public IHttpActionResult getAllCategories()
        {
            ResponseDTO response = new ResponseDTO();
            IList<Category> list = jobService.getAllCategories();
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
        [Authorize(Roles = "ROLE_STUDENT")]
        [Route("applied-jobs")]
        public IHttpActionResult getApplliedJobs()
        {
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int studentId = HttpContextUtils.getUserID(claims);
            ResponseDTO response = new ResponseDTO();
            IList<AppliedJobDTO> list = jobService.getAppliedJobByStudentId(studentId);
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
        [Authorize(Roles = "ROLE_RECRUITER")]
        [Route("posted-jobs")]
        public IHttpActionResult getPostedJobs()
        {
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int recruiterId = HttpContextUtils.getUserID(claims);
            ResponseDTO response = new ResponseDTO();
            IList<Job> list = jobService.getPostedJobByRecruiterId(recruiterId);
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
        [Authorize(Roles = "ROLE_STUDENT")]
        [Route("suggest")]
        public IHttpActionResult getSuggestions()
        {
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int studentId = HttpContextUtils.getUserID(claims);
            String lastAppliedJobString = studentService.getLastAppliedJobString(studentId);
            ResponseDTO response = new ResponseDTO();
            if (lastAppliedJobString == null || lastAppliedJobString.IsEmpty())
            {
                response.message = "This user hasn't performed apply action";
                return Ok(response);
            }
            else
            {
                IList<Job> result = jobService.getSuggestedJob(lastAppliedJobString, studentId);
                if (result.Count == 0)
                {
                    response.message = "No data";
                    return Ok(response);
                }
                response.message = "OK";
                response.data = result;
                return Ok(response);
            }
        }

        [HttpPost]
        [Route("apply")]
        [Authorize(Roles = "ROLE_STUDENT")]
        public IHttpActionResult applyAJob([FromBody] ApplyAJobDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("invalid_id", "Id is an integer");
                return BadRequest(ModelState);
            }
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int jobId = dto.id;
            int studentId = HttpContextUtils.getUserID(claims);
            int applyState = jobService.applyAJob(jobId, studentId);
            ResponseDTO response = new ResponseDTO();
            switch (applyState)
            {
                case 1:
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("This job doesn't exist");
                case 3:
                    return BadRequest("This student doesn't exist");
                case 4:
                    return BadRequest("This job is over 30 days");
                case 5:
                    return BadRequest("This job is pending");
                case 6:
                    return InternalServerError();
                case 7:
                    return BadRequest("This student already applied for this job");
            }
            return InternalServerError();
        }

        [HttpGet]
        [Route("pending-jobs")]
        [Authorize(Roles = "ROLE_STAFF")]
        public IHttpActionResult getAllPendingJobs()
        {
            ResponseDTO response = new ResponseDTO();
            IList<Job> list = jobService.getAllPendingJobs();
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
        [Route("approved-jobs")]
        [AllowAnonymous]
        public IHttpActionResult getAllApprovedJobs()
        {
            ResponseDTO response = new ResponseDTO();
            IList<Job> list = jobService.getAllApprovedJobs();
            if (list.Count == 0)
            {
                response.message = "No data";
                return Ok(response);
            }
            response.message = "OK";
            response.data = list;
            return Ok(response);
        }

        [HttpPut]
        [Route("approve")]
        [Authorize(Roles = "ROLE_STAFF")]
        public IHttpActionResult approveAJob([FromBody] ApproveAJobDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("invalid_id", "Id is an integer");
                return BadRequest(ModelState);
            }
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int jobId = dto.id;
            int staffId = HttpContextUtils.getUserID(claims);
            bool approveState = jobService.approveAJob(jobId, staffId);
            if (!approveState)
            {
                return BadRequest("Error occured");
            }
            ResponseDTO response = new ResponseDTO();
            response.message = "OK";
            return Ok(response);

        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public IHttpActionResult getAllJobs()
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
        [AllowAnonymous]
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

        [HttpGet]
        [Authorize(Roles = "ROLE_STUDENT")]
        [Route("saved-jobs")]
        public IHttpActionResult getSavedJobs()
        {
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int studentId = HttpContextUtils.getUserID(claims);
            IList<ReturnSavedJobDTO> result = studentService.getSavedJobs(studentId);
            ResponseDTO response = new ResponseDTO();
            if (result.Count == 0)
            {
                response.message = "No data";
                return Ok(response);
            }
            response.message = "OK";
            response.data = result;
            return Ok(response);
        }

        [HttpPost]
        [Authorize(Roles = "ROLE_STUDENT")]
        [Route("save")]
        public IHttpActionResult saveAJob([FromBody] SaveAJobDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("invalid_id", "Id is an integer");
                return BadRequest(ModelState);
            }
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int jobId = dto.jobId;
            int studentId = HttpContextUtils.getUserID(claims);
            int saveState = studentService.saveJob(jobId, studentId);
            switch (saveState)
            {
                case 1:
                    ResponseDTO response = new ResponseDTO();
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("Already saved");
                case 3:
                    return NotFound();
                case 4:
                    return InternalServerError();
            }
            return BadRequest();
        }

        [HttpDelete]
        [Authorize(Roles = "ROLE_STUDENT")]
        [Route("remove-saved-job/{jobId}")]
        public IHttpActionResult removeASavedJob([FromUri] int jobId)
        {
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int studentId = HttpContextUtils.getUserID(claims);
            int removeState = studentService.removeSavedJob(jobId, studentId);
            switch (removeState)
            {
                case 1:
                    ResponseDTO response = new ResponseDTO();
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return NotFound();
                case 3:
                    return InternalServerError();
            }
            return BadRequest();
        }


        [HttpGet]
        [Authorize(Roles = "ROLE_RECRUITER")]
        [Route("{jobId}/applied-students")]
        public IHttpActionResult getAppliedStudents([FromUri] int jobId)
        {
            ResponseDTO response = new ResponseDTO();
            IList<ReturnAppliedStudentDTO> list = studentService.getAppliedStudentsOfThisJob(jobId);
            if (list == null)
            {
                return InternalServerError();
            }
            else
            {
                if (list.Count == 0)
                {
                    response.message = "No data";
                    return Ok(response);
                }
                else
                {
                    response.message = "OK";
                }
            }
            response.data = list;
            return Ok(response);
        }

        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "ROLE_RECRUITER")]
        public IHttpActionResult createAJob([FromBody] CreateJobDTO dto)
        {
            if (dto.categories == null || dto.categories.Length == 0)
            {
                ModelState.AddModelError("dto.categories", "At least one category");
                return BadRequest(ModelState);
            }
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
            if (dto == null)
            {
                return BadRequest();
            }
            bool flag = false;
            if (dto.isEmpty())
            {
                ModelState.AddModelError("dto", "Data to update is empty!!!");
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
            if (dto.categories.Length == 0)
            {
                ModelState.AddModelError("dto.categories", "At least one category");
                flag = true;
            }
            if (flag)
            {
                return BadRequest(ModelState);
            }
            int updateState = jobService.update(dto, dto.id);
            switch (updateState)
            {
                case -1:
                    return NotFound();
                case 0:
                    ResponseDTO response = new ResponseDTO();
                    response.message = "OK";
                    return Ok(response);
                case 1:
                    return InternalServerError();
                case 2:
                    return BadRequest("This job was updated,in pending state");
                case 3:
                    ModelState.AddModelError("dto.salary", "salaryMin can't >= salaryMax or salaryMax <= salaryMin");
                    return BadRequest(ModelState);
            }
            return BadRequest();

        }
    }
}