using Capstone2021.DTO;
using Capstone2021.Services;
using Capstone2021.Services.Student;
using Capstone2021.Utils;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;

namespace Capstone2021.Controllers
{
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

        [HttpGet]
        [Route("self")]
        public IHttpActionResult getSelfInfo()
        {
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int id = HttpContextUtils.getUserID(claims);
            Student currentStudent = studentService.get(id);
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

                        studentService.updateImage(name, HttpContextUtils.getUserID(claims));
                        return Ok();

                    }
                    else
                        return BadRequest("Error occured while sending request to AWS S3");
                }
            }
            return BadRequest("Error occured");
        }
        [HttpPost]
        [Route("cv/create")]
        public IHttpActionResult createACv([FromBody] CreateCvDTO dto)
        {
            if (!ModelState.IsValid || dto == null)
            {
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
                return BadRequest("Already created");
            }
            return Ok(response);
        }
        [HttpPost]
        [Route("cv/upload-image")]
        public IHttpActionResult uploadCvImage()
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
                    string name = Guid.NewGuid().ToString() + "." + file.ContentType.Split('/')[1];
                    string myBucketName = "capstone2021-fpt"; //your s3 bucket name goes here  
                    string s3DirectoryName = "";
                    string s3FileName = @name;
                    bool a;
                    AmazonUploader myUploader = new AmazonUploader();
                    a = myUploader.sendMyFileToS3(st, myBucketName, s3DirectoryName, s3FileName);
                    if (a == true)
                    {

                        cvService.updateImage(name, HttpContextUtils.getUserID(claims));
                        return Ok();

                    }
                    else
                        return BadRequest("Error occured while sending request to AWS S3");
                }
            }
            return BadRequest("Error occured");
        }
        [HttpPut]
        [Route("cv/update")]
        public IHttpActionResult updateCv([FromBody] UpdateCvDTO cv)
        {
            if (!ModelState.IsValid || cv == null)
            {
                return BadRequest(ModelState);
            }

            ResponseDTO response = new ResponseDTO();
            bool saveState = cvService.update(cv, cv.id);
            if (saveState)
            {
                response.message = "Ok";
            }
            else
            {
                return BadRequest("Cannot Update");
            }
            return Ok(response);
        }
        [HttpGet]
        [Route("cv")]
        public IHttpActionResult getACv()
        {
            ResponseDTO response = new ResponseDTO();
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int studentId = HttpContextUtils.getUserID(claims);
            Cv currentCv = cvService.get(studentId);
            if(currentCv == null)
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
    }
}