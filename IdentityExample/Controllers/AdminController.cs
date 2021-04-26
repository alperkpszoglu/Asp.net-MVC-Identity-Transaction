using IdentityExample.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentityExample.Controllers
{
    [Authorize(Roles ="admin")]
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager;

        public AdminController()
        {
            var userStore = new UserStore<AppUser>(new AppIdentityContext());
            userManager = new UserManager<AppUser>(userStore);
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View(userManager.Users);
        }
    }
}