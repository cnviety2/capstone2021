using Microsoft.AspNet.Identity.EntityFramework;

namespace Capstone2021.Test
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base("AuthContext")
        {

        }
    }
}