using IdentityExample.Identity;
using IdentityExample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IdentityExample.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        
        public AccountController()
        {
            var userStore = new UserStore<AppUser>(new AppIdentityContext());
            userManager = new UserManager<AppUser>(userStore);

            userManager.PasswordValidator = new CustomValidator
            {
                RequiredLength=6,
                RequireUppercase=true
            };
            userManager.UserValidator = new UserValidator<AppUser>(userManager)
            {
                RequireUniqueEmail=true,
                AllowOnlyAlphanumericUserNames=false
            };
  
            
        }
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }
    
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        /*ASP.NET MVC ile hazırlanan sistemlerde login işlemi sonrası en son geldiğimiz sayfaya yönlendirme yapmak için,
          en son sayfa URL’i returnUrl adında bir değişkende tutulur.*/
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "Erişim Hakkınız yok." });
            }
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model,string returnUrl)
        {
            if (ModelState.IsValid) 
            {
                var user = userManager.Find(model.Username, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre yanlış");
                }
                else
                {
                    var authManager = HttpContext.GetOwinContext().Authentication;

                    var identity = userManager.CreateIdentity(user, "ApplicationCookie");
                    var authProperties = new AuthenticationProperties          //this is use owin.security
                    {
                        IsPersistent = true //the user can remember old information with this way
                    };

                    authManager.SignOut();
                    authManager.SignIn(authProperties, identity);

                    return Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl); //If returnurl is null, return to mainpage
                }
            }

            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid) 
            {
                var user = new AppUser();
                user.Email = model.Email;
                user.UserName = model.Username;

                var result = userManager.Create(user, model.Password);

                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "kullanıcı");
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }

                }

            }
            return View(model);
        }

        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();

            return Redirect("Login");
        }
    }

}