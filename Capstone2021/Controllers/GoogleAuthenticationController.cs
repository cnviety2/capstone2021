using Capstone2021.DTO;
using Capstone2021.Services;
using Capstone2021.Services.Student;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Capstone2021.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("google")]
    public class GoogleAuthenticationController : ApiController
    {
        private static readonly HttpClient client = new HttpClient();
        private StudentService studentService;

        public GoogleAuthenticationController()
        {
            studentService = new StudentServiceImpl();
        }

        [HttpGet]
        [Route("login")]
        public async Task<IHttpActionResult> test(string code)
        {
            var values = new Dictionary<string, string>
            {
                { "client_id", GoogleAuthenConstants.GOOGLE_CLIENT_ID },
                { "client_secret", GoogleAuthenConstants.GOOGLE_CLIENT_SECRET },
                { "redirect_uri", "https://localhost:44315/google/login" },
                { "grant_type", GoogleAuthenConstants.GOOGLE_GRANT_TYPE },
                { "code", code }
            };
            var content = new FormUrlEncodedContent(values);
            var response = await client.PostAsync(GoogleAuthenConstants.GOOGLE_LINK_GET_TOKEN, content);
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Unsuccessfull request");//request lên google ko success 
            }
            var responseString = await response.Content.ReadAsStringAsync();
            GoogleAccessTokenDTO accessTokenDTO = JsonConvert.DeserializeObject<GoogleAccessTokenDTO>(responseString);
            string accessToken = accessTokenDTO.access_token.Replace("\"", "");
            String link = GoogleAuthenConstants.GOOGLE_LINK_GET_USER_INFO + accessToken;
            response = await client.GetAsync(link);//request cùng với accesstoken 1 lần nữa để lấy data của user
            if (!response.IsSuccessStatusCode)
            {
                return BadRequest("Unsuccessfull request");
            }
            responseString = await response.Content.ReadAsStringAsync();
            GoogleAuthenDTO dto = JsonConvert.DeserializeObject<GoogleAuthenDTO>(responseString);
            Student student = new Student()
            {
                googleId = dto.id,
                avatar = dto.picture,
                gmail = dto.email
            };
            bool createState = studentService.create(student);
            if (createState)
                return Ok(dto);
            else return InternalServerError();
        }
    }
}