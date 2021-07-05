using Capstone2021.DTO;
using Capstone2021.Service;
using Capstone2021.Test.Repository;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

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

            ManagerService managerService = new ManagerServiceImpl();
            Manager manager = null;
            //lấy ra uri của request từ client
            Uri uri = context.Request.Uri;
            //tách phần query phía sau để biết đang cần xác thực ở table nào,VD:/token?role=manager là đang cần xác thực ở bảng manager,lúc đó sẽ gọi managerService để xác thực
            String query = uri.PathAndQuery.Split('=')[1];
            switch (query)
            {
                case "manager":
                    manager = managerService.login(new Manager() { username = context.UserName, password = context.Password });
                    if (manager == null)
                    {
                        context.SetError("invalid_grant", "The user name or password is incorrect.");
                        return;
                    }
                    break;
                default:
                    context.SetError("invalid_grant", "The syntax of query is incorrect");
                    return;
            }
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
            identity.AddClaim(new Claim(ClaimTypes.Role, manager.role));

            context.Validated(identity);

        }
    }
}