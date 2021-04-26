using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityExample.Identity
{
    public class AppIdentityContext:IdentityDbContext<AppUser>
    {
        public AppIdentityContext():base("IdentityConnection")
        {

        }
    }
}