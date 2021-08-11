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
        private RecruiterService recruiterService;

        public JobController()
        {
            jobService = new JobServiceImpl();
            studentService = new StudentServiceImpl();
            recruiterService = new RecruiterServiceImpl();
        }

        //api trả về list những category,check
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

        //Trả về list những job đã apply,có thể bị trùng vì apply 1 job nhiều lần bằng nhiều cv khác nhau,check
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
                response.message = "Không có dữ liệu";
                return Ok(response);
            }
            response.message = "OK";
            response.data = list;
            return Ok(response);
        }

        //Trả về những job đã đăng của recuriter này,check
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
                response.message = "Không có dữ liệu";
                return Ok(response);
            }
            response.message = "OK";
            response.data = list;
            return Ok(response);
        }

        //api trả về những job suggest từ job cuối cùng student apply,check
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
                response.message = "User chưa thực hiện nộp đơn vào 1 công việc";
                return Ok(response);
            }
            else
            {
                IList<Job> result = jobService.getSuggestedJob(lastAppliedJobString, studentId);
                if (result.Count == 0)
                {
                    response.message = "Không có dữ liệu";
                    return Ok(response);
                }
                response.message = "OK";
                response.data = result;
                return Ok(response);
            }
        }

        //api apply job của student,student gửi id của cv đi,check
        [HttpPost]
        [Route("apply")]
        [Authorize(Roles = "ROLE_STUDENT")]
        public IHttpActionResult applyAJob([FromBody] ApplyAJobDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("invalid_id", "Id là những con số");
                return BadRequest(ModelState);
            }
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int jobId = dto.jobId;
            int cvId = dto.cvId;
            int studentId = HttpContextUtils.getUserID(claims);
            int applyState = jobService.applyAJob(jobId, studentId, cvId);
            ResponseDTO response = new ResponseDTO();
            switch (applyState)
            {
                case 1:
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("Không tìm thấy công việc này");
                case 3:
                    return BadRequest("Không tìm thấy user này");
                case 4:
                    return BadRequest("Công việc đã quá 30 ngày");
                case 5:
                    return BadRequest("Công việc đang ở trạng thái chờ");
                case 6:
                    return InternalServerError();
                case 7:
                    return BadRequest("CV không tồn tại");
                case 8:
                    return BadRequest("Bạn đã gửi CV này đến nhà tuyển dụng rồi");
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
                response.message = "Không có dữ liệu";
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
                response.message = "Không có dữ liệu";
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
                ModelState.AddModelError("invalid_id", "Id là số nguyên");
                return BadRequest(ModelState);
            }
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int jobId = dto.id;
            int staffId = HttpContextUtils.getUserID(claims);
            bool approveState = jobService.approveAJob(jobId, staffId);
            if (!approveState)
            {
                return BadRequest("Lỗi xảy ra ");
            }
            ResponseDTO response = new ResponseDTO();
            response.message = "OK";
            return Ok(response);

        }

        //Trả về những job vừa đc approve mới nhất và không quá 30 ngày sort theo thời gian create,check
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
        [Route("")]
        [AllowAnonymous]
        public IHttpActionResult getAllWithPaging([FromUri] int page)
        {
            ResponseDTO response = new ResponseDTO();
            IList<Job> list = jobService.getAllWithPaging(page);
            if (list.Count == 0)
            {
                response.message = "Không có dữ liệu";
                return Ok(response);
            }
            response.message = "OK";
            response.data = list;
            return Ok(response);
        }
        [HttpGet]
        [Route("total-pages")]
        [AllowAnonymous]
        public IHttpActionResult getTotalPages()
        {
            ResponseDTO response = new ResponseDTO();
            int result = jobService.getTotalPages();
            response.message = "OK";
            response.data = result;
            return Ok(response);
        }

        //Trả về chi tiết thông tin 1 job,check 
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

        //Trả về những job đã lưu,check
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
                response.message = "Không có dữ liệu";
                return Ok(response);
            }
            response.message = "OK";
            response.data = result;
            return Ok(response);
        }

        //lưu 1 job dựa trên jobId,check
        [HttpPost]
        [Authorize(Roles = "ROLE_STUDENT")]
        [Route("save")]
        public IHttpActionResult saveAJob([FromBody] SaveAJobDTO dto)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("invalid_id", "Id là số nguyên");
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
                    return BadRequest("User đã lưu rồi");
                case 3:
                    return NotFound();
                case 4:
                    return InternalServerError();
            }
            return BadRequest();
        }

        //Xoá lưu 1 job trong list đã lưu,check
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

        //Trả về list những cv đã apply vào job này,check
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
                    response.message = "Không có dữ liệu";
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

        //Tạo mới 1 job của recruiter,tạm thời set status bằng 2,check
        [HttpPost]
        [Route("create")]
        [Authorize(Roles = "ROLE_RECRUITER")]
        public IHttpActionResult createAJob([FromBody] CreateJobDTO dto)
        {
            if (dto.categories == null || dto.categories.Length == 0)
            {
                ModelState.AddModelError("dto.categories", "Ít nhất 1 category");
                return BadRequest(ModelState);
            }
            if (dto.salaryMin >= dto.salaryMax)
            {
                ModelState.AddModelError("dto.salaryMin", "salaryMin không thể >= salaryMax");
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

        //Update 1 job của recruiter,ko thể update nếu đã đc duyệt,check
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
                ModelState.AddModelError("dto", "Dữ liệu không được bỏ trống hoàn toàn");
                flag = true;
            }
            if (dto.workingForm.HasValue && (dto.workingForm < 1 || dto.workingForm > 3))
            {
                ModelState.AddModelError("dto.workingForm", "`Part-time:1,Full-time:2,Cả 2:3");
                flag = true;
            }
            if (dto.location.HasValue && (dto.location < 1 || dto.location > 24))
            {
                ModelState.AddModelError("dto.location", "Quận 1 -> 12 : 1 -> 12,13 : bình tân,14 : bình thạnh,15 : gò vấp,16 : phú nhuận,17 : tân bình,18 : " +
            "tân phú,19 : thủ đức,20 : bình chánh,21 : cần giờ,22 : củ chi,23 : hóc môn,24 : nhà bè");
                flag = true;
            }
            if (dto.sex.HasValue && (dto.sex < 1 || dto.sex > 3))
            {
                ModelState.AddModelError("dto.sex", "Yêu cầu giới tính : 1 name,2 nữ,3 cả 2");
                flag = true;
            }
            if (dto.quantity.HasValue && dto.quantity < 1)
            {
                ModelState.AddModelError("dto.quantity", "Giá trị nhỏ nhất là 1");
                flag = true;
            }
            if (dto.salaryMin.HasValue && dto.salaryMin < 1)
            {
                ModelState.AddModelError("dto.salaryMin", "Giá trị nhỏ nhất là 1");
                flag = true;
            }
            if (dto.salaryMax.HasValue && dto.salaryMax < 1)
            {
                ModelState.AddModelError("dto.salaryMax", "Giá trị nhỏ nhất là 1");
                flag = true;
            }
            if ((dto.salaryMax.HasValue && dto.salaryMin.HasValue) && dto.salaryMin >= dto.salaryMax)
            {
                ModelState.AddModelError("dto.salaryMin", "salaryMin không thể >= salaryMax");
                flag = true;
            }
            if (dto.categories != null && dto.categories.Length == 0)
            {
                ModelState.AddModelError("dto.categories", "Ít nhất chọn 1");
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
                    return BadRequest("Công việc này đã duyệt,không thể update nữa");
                case 3:
                    ModelState.AddModelError("dto.salary", "salaryMin không thể >= salaryMax hoặc salaryMax <= salaryMin");
                    return BadRequest(ModelState);
            }
            return BadRequest();
        }

        //Xóa những job tạo bởi recruiter,check
        [HttpDelete]
        [Route("remove-posted-job/{jobId}")]
        [Authorize(Roles = "ROLE_RECRUITER")]
        public IHttpActionResult removePostedJobs([FromUri] int jobId)
        {
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int recruiterId = HttpContextUtils.getUserID(claims);
            int removeState = recruiterService.removeAJob(recruiterId, jobId);
            switch (removeState)
            {
                case 1:
                    ResponseDTO response = new ResponseDTO();
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("Không tồn tại user");
                case 3:
                    return BadRequest("Không tồn tại công việc này");
                case 4:
                    return InternalServerError();
                default:
                    return InternalServerError();
            }
        }
    }
}