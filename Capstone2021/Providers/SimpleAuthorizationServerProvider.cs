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

    }
}