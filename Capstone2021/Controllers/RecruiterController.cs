using Capstone2021.DTO;
using Capstone2021.Services;
using Capstone2021.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;

namespace Capstone2021.Controllers
{
    [RoutePrefix("recruiter")]
    [Authorize]//chỉ có những user có role là recruiter chứa trong HttpContext là được sử dụng những api này
    public class RecruiterController : ApiController
    {
        private readonly RecruiterService _recruiterService;
        public RecruiterController()
        {
            _recruiterService = new RecruiterServiceImpl();
        }
        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public IHttpActionResult create([FromBody] CreateRecruiterDTO recruiter)
        {
            ResponseDTO response = new ResponseDTO();
            if (!ModelState.IsValid || recruiter == null)
            {
                return BadRequest(ModelState);
            }

            recruiter.password = Crypto.HashPassword(recruiter.password.Trim());
            if (StringUtils.isContainSpecialCharacter(recruiter.username) == true)
            {
                response.message = "Username not contain sepecial character";
                return Ok(response);
            }
            else
            {
                Recruiter model = RecruiterMapper.mapFromDto(recruiter);
                bool saveState = _recruiterService.create(model);
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
        }

        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "ROLE_RECRUITER")]
        public IHttpActionResult updateInformation([FromBody] UpdateInformationRecruiterDTO recruiter)
        {
            if (!ModelState.IsValid || recruiter == null)
            {
                return BadRequest(ModelState);
            }

            ResponseDTO response = new ResponseDTO();
            String currentUser = HttpContextUtils.getUsername(HttpContext.Current.User.Identity);
            bool saveState = _recruiterService.update(recruiter, currentUser);
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
        [Route("update/password")]
        [HttpPut]
        [Authorize(Roles = "ROLE_RECRUITER")]
        public IHttpActionResult updatePassword([FromBody] UpdatePasswordRecruiterDTO dto)
        {
            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }
            ResponseDTO response = new ResponseDTO();
            string currentUser = HttpContextUtils.getUsername(HttpContext.Current.User.Identity);
            bool updateState = _recruiterService.updatePassword(dto.password, currentUser);
            if (updateState)
            {
                response.message = "OK";
            }
            else
            {
                return InternalServerError();
            }
            return Ok(response);
        }
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "ROLE_ADMIN")]
        public IHttpActionResult getAllRecruiter()
        {
            ResponseDTO response = new ResponseDTO();
            IList<Recruiter> list = _recruiterService.getAll();
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
        [Authorize(Roles = "ROLE_RECRUITER")]
        public IHttpActionResult getARecruiter([FromUri] int id)
        {
            ResponseDTO response = new ResponseDTO();
            Recruiter recruiter = _recruiterService.get(id);
            if (recruiter == null)
            {
                return NotFound();
            }
            else
            {
                response.message = "OK";
            }
            response.data = recruiter;
            return Ok(response);
        }
        [HttpPost]
        [Route("upload-image")]
        [Authorize(Roles = "ROLE_RECRUITER")]
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
                    ResponseDTO response = new ResponseDTO();
                    ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
                    string name = Guid.NewGuid().ToString() + "." + file.ContentType.Split('/')[1];
                    string myBucketName = "capstone2021-fpt";//your s3 bucket name goes here  
                    string s3DirectoryName = "";
                    string s3FileName = @name;
                    bool a;
                    AmazonUploader myUploader = new AmazonUploader();
                    a = myUploader.sendMyFileToS3(st, myBucketName, s3DirectoryName, s3FileName);
                    if (a == true)
                    {
                        _recruiterService.updateImage(name, HttpContextUtils.getUserID(claims));
                        response.message = "Upload avatar successfull";
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest("Error occured while sending request to AWS S3");

                    }
                }
            }
            return BadRequest("Error occured");
        }
    }
}