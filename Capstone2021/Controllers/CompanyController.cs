using Capstone2021.DTO;
using Capstone2021.Services;
using Capstone2021.Utils;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.WebPages;

namespace Capstone2021.Controllers
{
    //check
    public class CompanyController : ApiController
    {
        private CompanyService companyService;

        public CompanyController()
        {
            companyService = new CompanyServiceImpl();
        }

        //Trả về thông tin công ty dựa trên recruiterId,check
        [HttpGet]
        [Route("company/{recruiterId}")]
        [AllowAnonymous]
        public IHttpActionResult getACompanyInfoByRecruiterId([FromUri] int recruiterId)
        {
            ReturnCompanyDTO result = companyService.getCompanyByRecruiterId(recruiterId);
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                ResponseDTO response = new ResponseDTO();
                response.message = "OK";
                response.data = result;
                return Ok(response);
            }
        }

        //trả về company data của recruiter này,check
        [HttpGet]
        [Route("recruiter/company/self")]
        [Authorize(Roles = "ROLE_RECRUITER")]
        public IHttpActionResult getSelfCompany()
        {
            ResponseDTO response = new ResponseDTO();
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int recruiterId = HttpContextUtils.getUserID(claims);
            Company company = companyService.getSelfCompany(recruiterId);
            if (company == null)
            {
                return NotFound();
            }
            else
            {
                response.message = "OK";
            }
            response.data = company;
            return Ok(response);
        }

        [HttpPost]
        [Route("recruiter/company/create/v2")]
        [Authorize(Roles = "ROLE_RECRUITER")]
        public IHttpActionResult createSelfCompanyInfoV2()
        {
            NameValueCollection formValue = HttpContext.Current.Request.Form;
            CreateCompanyDTO dto = new CreateCompanyDTO();
            dto.name = formValue.Get("name");
            dto.headquaters = formValue.Get("headquarters");
            dto.website = formValue.Get("website");
            dto.description = formValue.Get("description");
            if (dto.isEmpty())
            {
                return BadRequest("Dữ liệu không được trống");
            }
            if (dto.website != null && !dto.website.IsEmpty())
            {
                if (!StringUtils.isValidHttpUrl(dto.website))
                {
                    return BadRequest("Url website chưa đúng,VD:http://abc.com");
                }
            }
            var file = HttpContext.Current.Request.Files.Count > 0 ? HttpContext.Current.Request.Files[0] : null;
            if (file != null && file.ContentLength > 0)
            {
                if (!file.ContentType.Equals("image/jpeg") && !file.ContentType.Equals("image/png"))
                {
                    return BadRequest("Only jpeg or png");
                }
                using (Stream st = file.InputStream)
                {
                    string name = Guid.NewGuid().ToString() + "." + file.ContentType.Split('/')[1];
                    string myBucketName = "capstone2021-fpt";//your s3 bucket name goes here  
                    string s3DirectoryName = "";
                    string s3FileName = @name;
                    bool a;
                    AmazonUploader myUploader = new AmazonUploader();
                    a = myUploader.sendMyFileToS3(st, myBucketName, s3DirectoryName, s3FileName);
                    String imgUrl = null;
                    if (a == true)
                    {
                        imgUrl = "https://capstone2021-fpt.s3.ap-southeast-1.amazonaws.com/" + name;
                    }
                    else
                    {
                        imgUrl = "";
                        return BadRequest("Lỗi,không thể upload ảnh lên server AWS");
                    }
                    dto.avatar = imgUrl;
                }
            }
            ResponseDTO response = new ResponseDTO();
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int recruiterId = HttpContextUtils.getUserID(claims);
            int createState = companyService.createNewCompanyInfo(dto, recruiterId);
            switch (createState)
            {
                case 1:
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("Đã tạo rồi");
                case 3:
                    return InternalServerError();
                default:
                    return InternalServerError();
            }
        }

        //Tạo mới info company của recruiter này,check
        [HttpPost]
        [Route("recruiter/company/create")]
        [Authorize(Roles = "ROLE_RECRUITER")]
        public IHttpActionResult createSelfCompanyInfo([FromBody] CreateCompanyDTO dto)
        {
            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }
            if (dto.website != null && !dto.website.IsEmpty())
            {
                if (!StringUtils.isValidHttpUrl(dto.website))
                {
                    return BadRequest("Url website chưa đúng,VD:http://abc.com");
                }
            }
            ResponseDTO response = new ResponseDTO();
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int recruiterId = HttpContextUtils.getUserID(claims);
            int createState = companyService.createNewCompanyInfo(dto, recruiterId);
            switch (createState)
            {
                case 1:
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("Đã tạo rồi");
                case 3:
                    return InternalServerError();
                default:
                    return InternalServerError();
            }
        }

        //upload ảnh của company,check
        [HttpPost]
        [Route("recruiter/company/upload-image")]
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
                        string imgUrl = companyService.updateImage(name, HttpContextUtils.getUserID(claims));
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

        //Update lại thông tin company của recruiter này,check
        [HttpPut]
        [Route("recruiter/company/update")]
        [Authorize(Roles = "ROLE_RECRUITER")]
        public IHttpActionResult updateSelfCompanyInfo([FromBody] UpdateCompanyDTO dto)
        {
            if (!ModelState.IsValid || dto == null)
            {
                return BadRequest(ModelState);
            }
            if (dto.isEmpty())
            {
                return BadRequest("Data không được trống");
            }
            if (dto.website != null && !dto.website.IsEmpty())
            {
                if (!StringUtils.isValidHttpUrl(dto.website))
                {
                    return BadRequest("Url website chưa đúng,VD:http://abc.com");
                }
            }
            ClaimsPrincipal claims = Request.GetRequestContext().Principal as ClaimsPrincipal;
            int recruiterId = HttpContextUtils.getUserID(claims);
            int updateState = companyService.updateCompanyInfo(dto, recruiterId);
            switch (updateState)
            {
                case 1:
                    ResponseDTO response = new ResponseDTO();
                    response.message = "OK";
                    return Ok(response);
                case 2:
                    return BadRequest("Không tồn tại user này");
                case 3:
                    return BadRequest("Chưa tạo thông tin công ty");
                case 4:
                    return InternalServerError();
                default:
                    return InternalServerError();
            }

        }
    }
}