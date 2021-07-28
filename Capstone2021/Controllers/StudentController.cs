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

        public StudentController()
        {
            studentService = new StudentServiceImpl();
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
    }
}