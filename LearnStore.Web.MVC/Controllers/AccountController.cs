using LearnStore.Application.Commands.AuthCommands;
using LearnStore.Web.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LearnStore.Web.MVC.Controllers
{
    public class AccountController(IMediator Mediator) : Controller
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

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
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
                ModelState.AddModelError(string.Empty, registerResult.Error ?? "An error occurred while registering.");
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
                ModelState.AddModelError(string.Empty, loginResult.Error ?? "An error occurred while logging in.");
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
                ModelState.AddModelError(string.Empty, registerResult.Error ?? "An error occurred while registering.");
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
                ModelState.AddModelError(string.Empty, loginResult.Error ?? "An error occurred while logging in.");
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
