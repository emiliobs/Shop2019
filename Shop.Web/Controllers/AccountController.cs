using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shop.Web.Data.Entities;
using Shop.Web.Helpers;
using Shop.Web.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Controllers
{

    public class AccountController : Controller
    {
        private readonly IUserHelper _userHelper;

        public AccountController(IUserHelper userHelper)
        {
            _userHelper = userHelper;
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterNewUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userHelper.GetUserBiEmailAsync(model.Username);
                if (user == null)
                {
                    user = new User
                    {
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Email = model.Username,
                        UserName = model.Username,
                    };

                    var result = await _userHelper.AddUserAsync(user, model.Password);
                    if (result != IdentityResult.Success)
                    {
                        ModelState.AddModelError(string.Empty, "The User couldn't be created.");
                        return View(model);
                    }

                    //Aqui logeo al useario registrado:
                    var loginResult = await _userHelper.LoginAsync(new LoginViewModel
                    {
                        Password = model.Password,
                        RememberMe = false,
                        UserName = model.Username,
                    });

                    if (loginResult.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    ModelState.AddModelError(string.Empty, "The User couldn't be Login.");
                    return View(model);
                }

                ModelState.AddModelError(string.Empty, "The Username is already registered.");

            }

            return View(model);
        }

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userHelper.LoginAsync(model);
                if (result.Succeeded)
                {
                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }

                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Failed to Login.");
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _userHelper.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}