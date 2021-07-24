using Capstone2021.DTO;
using Capstone2021.Service;
using Capstone2021.Services;
using Capstone2021.Services.Student;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace Capstone2021.Test.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private static readonly HttpClient client = new HttpClient();
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            //lấy ra uri của request từ client
            Uri uri = context.Request.Uri;
            //tách phần query phía sau để biết đang cần xác thực ở table nào,VD:/token?role=manager là đang cần xác thực ở bảng manager,lúc đó sẽ gọi managerService để xác thực
            String query;
            String role;
            try
            {
                query = uri.PathAndQuery.Split('=')[1];
                role = uri.PathAndQuery.Split('=')[0];
            }
            catch (Exception e)
            {
                context.SetError("invalid_uri", "The uri is incorrect");
                return;
            }
            if (!role.Equals("/token?role"))
            {
                context.SetError("invalid_uri", "The uri is incorrect");
                return;
            }
            switch (query)
            {
                case "manager":
                    ManagerService managerService = new ManagerServiceImpl();
                    Manager manager = managerService.login(context.UserName, context.Password);
                    if (manager == null)
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect");
                        return;
                    }
                    if (isBanned(manager))
                    {
                        context.SetError("invalid_state", "This account is being banned");
                        return;
                    }
                    addClaimsToIdentity(identity, context.UserName, manager.role, manager.id);
                    break;
                case "recruiter":
                    RecruiterService recruiterService = new RecruiterServiceImpl();
                    Recruiter recruiter = recruiterService.login(context.UserName, context.Password);
                    if (recruiter == null)
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect");
                        return;
                    }
                    if (isBanned(recruiter))
                    {
                        context.SetError("invalid_state", "This account is being banned");
                        return;
                    }
                    addClaimsToIdentity(identity, context.UserName, recruiter.role, recruiter.id);
                    break;
                case "student":
                    StudentService studentService = new StudentServiceImpl();
                    Student obj = new Student();
                    obj.gmail = context.UserName;
                    Dictionary<string, string> dict = GetBodyParameters(context.Request);
                    obj.googleId = dict["google_id"];
                    obj.avatar = dict["avatar"];
                    if (obj.googleId == null || obj.googleId.IsEmpty())
                    {
                        context.SetError("invalid_request", "Missing data");
                        return;
                    }
                    Student student = studentService.login(obj);
                    if (student == null)
                    {
                        context.SetError("invalid_state", "Server error");
                        return;
                    }
                    if (isBanned(student))
                    {
                        context.SetError("invalid_state", "This account is being banned");
                        return;
                    }
                    addClaimsToIdentity(identity, context.UserName, "ROLE_STUDENT", student.id, student.googleId);
                    break;
                default:
                    context.SetError("invalid_uri", "The syntax of query is incorrect");
                    return;
            }
            context.Validated(identity);

        }

        private void addClaimsToIdentity(ClaimsIdentity identity, string username, string role)
        {
            identity.AddClaim(new Claim(ClaimTypes.Name, username));//set username vào HttpContext để biết được user nào đang gửi request 
            identity.AddClaim(new Claim(ClaimTypes.Role, role));//set role vào HttpContext để phân quyền đc phép sử dụng những api nào
        }

        private void addClaimsToIdentity(ClaimsIdentity identity, string username, string role, int id)
        {
            identity.AddClaim(new Claim(ClaimTypes.Name, username));//set username vào HttpContext để biết được user nào đang gửi request 
            identity.AddClaim(new Claim(ClaimTypes.Role, role));//set role vào HttpContext để phân quyền đc phép sử dụng những api nào
            identity.AddClaim(new Claim("id", id.ToString()));//set id vào claim,ko sử dụng HttpContext để lấy được,phải làm cách khác
        }

        private void addClaimsToIdentity(ClaimsIdentity identity, string username, string role, int id, string googleId)
        {
            identity.AddClaim(new Claim(ClaimTypes.Name, username));
            identity.AddClaim(new Claim(ClaimTypes.Role, role));
            identity.AddClaim(new Claim("id", id.ToString()));
            identity.AddClaim(new Claim("google_id", googleId));
        }

        /// <summary>
        /// Kiểm tra status của manager trả về có bị ban hay ko,true nếu bị ban
        /// </summary>
        /// <param name="manager"></param>
        /// <returns></returns>
        private bool isBanned(Manager manager)
        {
            if (manager.isBanned == true)
                return true;
            else
                return false;
        }

        private bool isBanned(Recruiter recruiter)
        {
            if (recruiter.isBanned == true)
                return true;
            else
                return false;
        }

        private bool isBanned(Student stdent)
        {
            if (stdent.isBanned == true)
                return true;
            else
                return false;
        }

        private Dictionary<string, string> GetBodyParameters(IOwinRequest request)
        {
            var dictionary = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);

            var formCollectionTask = request.ReadFormAsync();

            formCollectionTask.Wait();

            foreach (var pair in formCollectionTask.Result)
            {
                var value = GetJoinedValue(pair.Value);

                dictionary.Add(pair.Key, value);
            }

            return dictionary;
        }
        private string GetJoinedValue(string[] value)
        {
            if (value != null)
                return string.Join(",", value);

            return null;
        }

    }
}