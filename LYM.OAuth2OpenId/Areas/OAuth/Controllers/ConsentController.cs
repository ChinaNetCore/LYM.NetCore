// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Services;
using IdentityServer4.Stores;
using LYM.OAuth2OpenId.IdrServices;
using LYM.OAuth2OpenId.IdrServices.Consent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LYM.OAuth2OpenId.Areas.OAuth.Controllers
{
    /// <summary>
    /// 授权同意页面
    /// </summary>
    [SecurityHeaders]
    public class ConsentController : Controller
    {
        private readonly ConsentService _consent;
       
        public ConsentController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore,
            ILogger<ConsentController> logger)
        {
            _consent = new ConsentService(interaction, clientStore, resourceStore, logger);
        }

        /// <summary>
        /// 授权同意页面
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var vm = await _consent.BuildViewModelAsync(returnUrl);
            if (vm != null)
            {
                return View("Index", vm);
            }

           // return View("Error");
            return View();
        }

        /// <summary>
        /// 处理回发同意
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConsentInputModel model)
        {
            var result = await _consent.ProcessConsent(model);

            if (result.IsRedirect)
            {
                return Redirect(result.RedirectUri);
            }

            if (result.HasValidationError)
            {
                ModelState.AddModelError("", result.ValidationError);
            }

            if (result.ShowView)
            {
                return View("Index", result.ViewModel);
            }
            return View();
          //  return View("Error");
        }

       
        
    }
}