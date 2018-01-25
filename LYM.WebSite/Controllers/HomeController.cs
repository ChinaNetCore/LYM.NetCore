using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LYM.WebSite.Models;
using Microsoft.AspNetCore.Authorization;
using LYM.Core.Service;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace LYM.WebSite.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        private IUserService _userService;

        //private ICapPublisher _capPublisher;
        public HomeController(IUserService userService)
        {
            _userService = userService;
           
        }

        public async Task<IActionResult> Index()
        {

            return View();
        }

        public async Task<IActionResult> Iframes()
        {

            return View();
        }

       

        public async Task<IActionResult> About()
        {
            ViewData["Message"] = "这里是动态加载的数据";
            var lst = await _userService.GetList();
            return View(lst);
        }

       
       

        /// <summary>
        /// 联系页面用来订阅消息
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Contact()
        {

            ////添加消息事件
            //await _capPublisher.PublishAsync("lymtest.services", new LYM.Core.Model.User.UserLoginModel {
            //    UserName="消息订阅处理",
            //    UserPwd= "消息订阅处理"
            //});
            ViewData["Message"] = "联系页面用来发送订阅消息事件";
            return View();
        }

        public IActionResult Error()
        {

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


        [HttpPost]
        public void CoustomLogOut()
        {
            //HttpContext.SignOutAsync();
            HttpContext.SignOutAsync("lym.oauth.cookies");  //CookieAuthenticationDefaults.AuthenticationScheme
            HttpContext.SignOutAsync("oidc");
            //HttpContext.SignOutAsync("lym.website");

           // return Redirect("/");


        }

        //[AllowAnonymous, HttpPost, Route("/oidc/front-channel-logout-callback")]
        //public void ClearCookies()
        //{

        //    HttpContext.SignOutAsync("lym.website");
           


        //}


        
    }
}
