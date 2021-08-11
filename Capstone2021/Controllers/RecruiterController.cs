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
using System.Web.WebPages;

namespace Capstone2021.Controllers
{
    //check
    [RoutePrefix("recruiter")]
    [Authorize]//chỉ có những user có role là recruiter chứa trong HttpContext là được sử dụng những api này
    public class RecruiterController : ApiController
    {
        private readonly RecruiterService _recruiterService;
        private CvService cvService;
        public RecruiterController()
        {
            _recruiterService = new RecruiterServiceImpl();
            cvService = new CvServiceImpl();
        }

        //Đăng ký 1 register mới,check
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
                return BadRequest("Username không được chứa ký tự đặc biệt");
            }
            if ((recruiter.firstname != null && !recruiter.firstname.IsEmpty()) || (recruiter.lastName != null && !recruiter.lastName.IsEmpty()))
            {
                if (StringUtils.isContainSpecialCharacter(recruiter.firstname) || StringUtils.isContainSpecialCharacter(recruiter.lastName))
                    return BadRequest("Tên không được chứa ký tự đặc biệt");
            }

            int saveState = _recruiterService.register(recruiter);
            switch (saveState)
            {
                case 1:
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("Username bị trùng");
                case 3:
                    return InternalServerError();
                default:
                    return InternalServerError();
            }
        }

        //Update thông tin recruiter,check
        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "ROLE_RECRUITER")]
        public IHttpActionResult updateInformation([FromBody] UpdateInformationRecruiterDTO recruiter)
        {
            if (!ModelState.IsValid || recruiter == null)
            {
                return BadRequest(ModelState);
            }
            if (recruiter.isEmpty())
            {
                return BadRequest("Data không được trống");
            }
            if (recruiter.gmail != null && !recruiter.gmail.IsEmpty())
            {
                if (!StringUtils.isValidEmailFormat(recruiter.gmail))
                {
                    return BadRequest("Email không đúng định dạng");
                }
            }
            if (recruiter.firstName != null && !recruiter.firstName.IsEmpty())
            {
                if (StringUtils.isContainSpecialCharacter(recruiter.firstName))
                {
                    return BadRequest("Tên không được chứa ký tự đặc biệt");
                }
            }
            if (recruiter.lastName != null && !recruiter.lastName.IsEmpty())
            {
                if (StringUtils.isContainSpecialCharacter(recruiter.lastName))
                {
                    return BadRequest("Tên không được chứa ký tự đặc biệt");
                }
            }
            if (recruiter.phone != null && !recruiter.phone.IsEmpty())
            {
                if (!StringUtils.isDigitsOnly(recruiter.phone))
                {
                    return BadRequest("SĐT chỉ chứa số");
                }
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
                response.message = "Xảy ra lỗi";
            }
            return Ok(response);
        }

        //update lại mât khẩu,check
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
        //test
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

        //Trả về thông tin 1 recruiter dựa trên id,check
        [HttpGet]
        [Route("{id:int:min(0)}")]
        [AllowAnonymous]
        public IHttpActionResult getARecruiter([FromUri] int id)
        {
            ResponseDTO response = new ResponseDTO();
            ReturnRecruiterDTO recruiter = _recruiterService.getById(id);
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

        //upload ảnh của recruiter,check
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
                        string imgUrl = _recruiterService.updateImage(name, HttpContextUtils.getUserID(claims));
                        response.message = "Upload avatar thành công";
                        response.data = imgUrl;
                        return Ok(response);
                    }
                    else
                    {
                        return BadRequest("Lỗi xảy ra khi upload ảnh lên aws");

                    }
                }
            }
            return BadRequest("Lỗi xảy ra");
        }

        //Trả về data của recruiter gửi request,check
        [HttpGet]
        [Route("self")]
        public IHttpActionResult getSelfInfo()
        {
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int id = HttpContextUtils.getUserID(claims);
            Recruiter currentRecruiter = _recruiterService.get(id);
            ResponseDTO response = new ResponseDTO();
            if (currentRecruiter == null)
            {
                return NotFound();
            }
            else
            {
                response.message = "OK";
            }
            response.data = currentRecruiter;
            return Ok(response);
        }

        [HttpGet]
        [Route("search-cvs")]
        public IHttpActionResult searchCvs([FromUri] string keyword)
        {
            IList<Cv> result = null;
            if (keyword == null || keyword.IsEmpty())
            {
                result = cvService.getAllPublicCvs();
            }
            else
            {
                result = cvService.searchCvs(keyword);
            }
            ResponseDTO response = new ResponseDTO();
            if (result.Count == 0)
            {
                response.message = "Không tìm thấy kết quả";
            }
            else
            {
                response.data = result;
                response.message = "OK";
            }
            return Ok(response);
        }
    }
}