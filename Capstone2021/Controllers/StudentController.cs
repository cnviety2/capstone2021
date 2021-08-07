using Capstone2021.DTO;
using Capstone2021.Services;
using Capstone2021.Services.Student;
using Capstone2021.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.WebPages;

namespace Capstone2021.Controllers
{
    //check
    [RoutePrefix("student")]
    [Authorize(Roles = "ROLE_STUDENT")]
    public class StudentController : ApiController
    {
        private StudentService studentService;
        private CvService cvService;

        public StudentController()
        {
            studentService = new StudentServiceImpl();
            cvService = new CvServiceImpl();
        }

        //Trả về data của student gửi request,có list cv,check
        [HttpGet]
        [Route("self")]
        public IHttpActionResult getSelfInfo()
        {
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int id = HttpContextUtils.getUserID(claims);
            ReturnStudentDTO currentStudent = studentService.getSelfInfo(id);
            ResponseDTO response = new ResponseDTO();
            if (currentStudent == null)
            {
                return NotFound();
            }
            else
            {
                response.message = "OK";
            }
            response.data = currentStudent;
            return Ok(response);
        }

        //api upload ảnh lên profile của student,check
        [HttpPost]
        [Route("upload-image")]
        public IHttpActionResult upload()
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;

            if (file != null && file.ContentLength > 0)
            {
                if (!file.ContentType.Equals("image/jpeg") && !file.ContentType.Equals("image/png"))
                {
                    return BadRequest("Only jpeg or png");
                }
                using (Stream st = file.InputStream)
                {
                    ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
                    string googleId = HttpContextUtils.getGoogleID(claims);
                    string name = Guid.NewGuid().ToString() + googleId + "." + file.ContentType.Split('/')[1];
                    //string name = Path.GetFileName(file.FileName);
                    string myBucketName = "capstone2021-fpt"; //your s3 bucket name goes here  
                    string s3DirectoryName = "";
                    string s3FileName = @name;
                    bool a;
                    AmazonUploader myUploader = new AmazonUploader();
                    a = myUploader.sendMyFileToS3(st, myBucketName, s3DirectoryName, s3FileName);
                    if (a == true)
                    {
                        string imgUrl = studentService.updateImage(name, HttpContextUtils.getUserID(claims));
                        ResponseDTO response = new ResponseDTO();
                        response.message = "Upload avatar thành công";
                        response.data = imgUrl;
                        return Ok(response);

                    }
                    else
                        return BadRequest("Lỗi xảy ra khi upload ảnh lên aws");
                }
            }
            return BadRequest("Lỗi xảy ra");
        }

        //api tạo mới 1 cv,check
        [HttpPost]
        [Route("cv/create")]
        public IHttpActionResult createACv([FromBody] CreateCvDTO dto)
        {
            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }
            //dob format yyyy-MM-dd
            if (!DateTimeUtils.is18Plus(dto.dob))
            {
                ModelState.AddModelError("dto.dob", "Phải trên 18 tuổi");
                return BadRequest(ModelState);
            }

            ResponseDTO response = new ResponseDTO();
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;

            int id = HttpContextUtils.getUserID(claims);
            bool createState = cvService.create(dto, id);
            if (createState)
            {
                response.message = "OK";
            }
            else
            {
                return BadRequest("Lỗi xảy ra");
            }
            return Ok(response);
        }

        //api upload ảnh lên 1 cv,check
        [HttpPost]
        [Route("cv/{cvId}/upload-image")]
        public IHttpActionResult uploadCvImage([FromUri] int cvId)
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;

            if (file != null && file.ContentLength > 0)
            {
                if (!file.ContentType.Equals("image/jpeg") && !file.ContentType.Equals("image/png"))
                {
                    return BadRequest("Only jpeg or png");
                }
                using (Stream st = file.InputStream)
                {
                    ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
                    string googleId = HttpContextUtils.getGoogleID(claims);
                    string name = Guid.NewGuid().ToString() + googleId + "." + file.ContentType.Split('/')[1];
                    string myBucketName = "capstone2021-fpt"; //your s3 bucket name goes here  
                    string s3DirectoryName = "";
                    string s3FileName = @name;
                    bool a;
                    AmazonUploader myUploader = new AmazonUploader();
                    a = myUploader.sendMyFileToS3(st, myBucketName, s3DirectoryName, s3FileName);
                    if (a == true)
                    {
                        string imgUrl = cvService.updateImage(name, HttpContextUtils.getUserID(claims), cvId);
                        ResponseDTO response = new ResponseDTO();
                        response.message = "Upload avatar thành công";
                        response.data = imgUrl;
                        if (imgUrl.IsEmpty())
                        {
                            return BadRequest("Upload không thành công");
                        }
                        return Ok(response);

                    }
                    else
                        return BadRequest("Lỗi xảy ra khi upload ảnh lên aws");
                }
            }
            return BadRequest("Lỗi xảy ra");
        }

        //api update cv của student gửi request check
        [HttpPut]
        [Route("cv/update")]
        public IHttpActionResult updateCv([FromBody] UpdateCvDTO cv)//id của cv nằm bên trong body
        {
            //bool flag = false;
            if (cv == null || cv.isEmpty())
            {
                ModelState.AddModelError("dto", "Dữ liệu không được bỏ trống tất cả");
                return BadRequest(ModelState);
            }
            if (cv.dob != null && !cv.dob.IsEmpty())
            {
                if (!DateTimeUtils.is18Plus(cv.dob))
                {
                    ModelState.AddModelError("dto.dob", "Phải trên 18 tuổi");
                    return BadRequest(ModelState);
                }

            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            ResponseDTO response = new ResponseDTO();
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int studentId = HttpContextUtils.getUserID(claims);
            int saveState = cvService.update(cv, studentId);
            switch (saveState)
            {
                case 1:
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("CV này không tồn tại");
                case 3:
                    return InternalServerError();
            }
            return InternalServerError();
        }

        //api trả về chi tiết cv của student gửi request,check
        [HttpGet]
        [Route("cv/{cvId}")]
        public IHttpActionResult getACv([FromUri] int cvId)
        {
            ResponseDTO response = new ResponseDTO();
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int studentId = HttpContextUtils.getUserID(claims);
            Cv currentCv = cvService.get(studentId, cvId);
            if (currentCv == null)
            {
                return NotFound();
            }
            else
            {
                response.message = "OK";
            }
            response.data = currentCv;
            return Ok(response);
        }

        //api trả về những cv của student gửi request,check
        [HttpGet]
        [Route("cv")]
        public IHttpActionResult getAllCvs()
        {
            ResponseDTO response = new ResponseDTO();
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int studentId = HttpContextUtils.getUserID(claims);
            IList<ReturnListCvDTO> result = cvService.getListCvs(studentId);
            if (result == null)
            {
                return InternalServerError();
            }
            if (result.Count == 0)
            {
                response.message = "No data";
            }
            else
            {
                response.message = "OK";
                response.data = result;
            }
            return Ok(response);
        }
    }
}