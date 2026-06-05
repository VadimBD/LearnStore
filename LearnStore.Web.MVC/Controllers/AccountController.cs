using LearnStore.Application.Commands.AuthCommands;

using LearnStore.Localization.Resources;
using LearnStore.Web.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace LearnStore.Web.MVC.Controllers
{
    public class AccountController(IMediator Mediator,IStringLocalizer<SharedResource> SharedLocalizer,IStringLocalizer<WebAppResource> WebAppLocalizer) : Controller
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
            try
            {
                var loginResult = await Mediator.Send(command);
                if (!loginResult.IsSuccess)
                {
                    ModelState.AddModelError(string.Empty, WebAppLocalizer["InvalidLogin"]);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(SharedLocalizer[ex.Message]);
            }

            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                return Redirect(model.ReturnUrl);

            return RedirectToAction("Index", "Home");

        }

        [HttpGet]
        [Route("RegisterCustomer")]
        public IActionResult RegisterCustomer()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("RegisterCustomer")]
        public async Task<IActionResult> RegisterCustomer(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var registerCommand = new RegisterCommand
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Roles = ["Customer"]
            };
            var registerResult = await Mediator.Send(registerCommand);

            if (!registerResult.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, registerResult.Error ?? WebAppLocalizer["AuthRegistrationFailed"]);
                return View(model);
            }
            var loginCommand = new LoginCommand
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Roles = ["Customer"]
            };
            var loginResult = await Mediator.Send(loginCommand);
            if (!loginResult.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, loginResult.Error ?? WebAppLocalizer["AuthLoginFailed"]);
                return View(model);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("RegisterSeller")]
        public IActionResult RegisterSeller()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("RegisterSeller")]
        public async Task<IActionResult> RegisterSeller(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var registerCommand = new RegisterCommand
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Roles = ["Seller"]
            };
            var registerResult = await Mediator.Send(registerCommand);

            if (!registerResult.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, registerResult.Error ?? WebAppLocalizer["AuthRegistrationFailed"]);
                return View(model);
            }
            var loginCommand = new LoginCommand
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                Roles = ["Seller"]
            };
            var loginResult = await Mediator.Send(loginCommand);
            if (!loginResult.IsSuccess)
            {
                ModelState.AddModelError(string.Empty, loginResult.Error ?? WebAppLocalizer["AuthLoginFailed"]);
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
