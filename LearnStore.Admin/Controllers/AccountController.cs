using LearnStore.Admin.Models;
using LearnStore.Application.Commands.AuthCommands;
using LearnStore.Localization.Resources;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LearnStore.Admin.Controllers
{
    public class AccountController (IMediator Mediator, IStringLocalizer<AdminResource> AdminLocalization , IStringLocalizer<SharedResource> SharedLocalization ) : Controller
    {

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var command = new LoginCommand
            {

                Email = model.Email,
                Password = model.Password,
            };
            var loginResult = await Mediator.Send(command);

            if (loginResult.IsSuccess)
            {
                if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    return Redirect(model.ReturnUrl);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, AdminLocalization["InvalidLogin"]);
            return View(model);
        }

        [HttpGet]
        [Route("RegisterAdmin")]
        public IActionResult RegisterAdmin()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("RegisterAdmin")]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var registerCommand = new RegisterCommand
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Roles = ["Admin"]
            };
            var registerResult = await Mediator.Send(registerCommand);

            if (!registerResult.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, registerResult.Error ?? AdminLocalization["AuthRegistrationFailed"]);
                return View(model);
            }
            var loginCommand = new LoginCommand
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Roles = ["Admin"]
            };
            var loginResult = await Mediator.Send(loginCommand);
            if (!loginResult.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, loginResult.Error ?? AdminLocalization["AuthLoginFailed"]);
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var logoutCommand = new LogoutCommand();
            await Mediator.Send(logoutCommand);

            return RedirectToAction("Index", "Home");
        }

    }
}
