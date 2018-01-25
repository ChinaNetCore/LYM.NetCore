using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LYM.OAuth2OpenId.Models;
using Microsoft.AspNetCore.Authorization;
using LYM.OAuth2OpenId.IdrServices;
using Microsoft.AspNetCore.Authentication;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace LYM.OAuth2OpenId.Controllers
{
    [SecurityHeaders]
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }



        public IActionResult CallBack()
        {
            return View();
        }


        [HttpPost]
        public void  CoustomLogOut()
        {
            //HttpContext.SignOutAsync();
  
            HttpContext.SignOutAsync("lym.oauth.cookies");  //CookieAuthenticationDefaults.AuthenticationScheme
            HttpContext.SignOutAsync("oidc");

            //return View();
            //  return Redirect("http://192.168.0.42:5000");


        }
    }
}
