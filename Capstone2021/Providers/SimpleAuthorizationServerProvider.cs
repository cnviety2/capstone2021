using Capstone2021.DTO;
using Capstone2021.Service;
using Capstone2021.Services;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Capstone2021.Test.Providers
{
    public class SimpleAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);

            ManagerService managerService = new ManagerServiceImpl();
            RecruiterService recruiterService = new RecruiterServiceImpl();

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
                    Manager manager = managerService.login(context.UserName, context.Password);
                    if (manager == null)
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        return;
                    }
                    addClaimsToIdentity(identity, context.UserName, manager.role);
                    break;
                case "recruiter":
                    Recruiter recruiter = recruiterService.login(context.UserName, context.Password);
                    if (recruiter == null)
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        return;
                    }
                    addClaimsToIdentity(identity, context.UserName, recruiter.role);
                    break;
                default:
                    context.SetError("invalid_uri", "The syntax of query is incorrect");
                    return;
            }
            /*identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));//set username vào HttpContext để biết được user nào đang gửi request 
            identity.AddClaim(new Claim(ClaimTypes.Role, manager.role));//set role vào HttpContext để phân quyền đc phép sử dụng những api nào*/


            context.Validated(identity);

        }

        private void addClaimsToIdentity(ClaimsIdentity identity, string username, string role)
        {
            identity.AddClaim(new Claim(ClaimTypes.Name, username));//set username vào HttpContext để biết được user nào đang gửi request 
            identity.AddClaim(new Claim(ClaimTypes.Role, role));//set role vào HttpContext để phân quyền đc phép sử dụng những api nào

        }
    }
}