using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using IdentityServer4.Services;
using LYM.OAuth2OpenId.IdrServices;

namespace LYM.OAuth2OpenId.Areas.OAuth.Controllers
{
    [SecurityHeaders]
    public class ErrorController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;

        public ErrorController(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }
        public async Task<IActionResult> Index(string errorId)
        {
            var vm = new IdrServices.Error.ErrorViewModel();
            IdentityServer4.Models.ErrorMessage message = await _interaction.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;
            }

            return View("Index", vm);
        }
    }
}