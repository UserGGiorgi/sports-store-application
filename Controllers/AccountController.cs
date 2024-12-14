using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    [Authorize]
    [Route("Account")]

    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [Route("Login")]
        [AllowAnonymous]
        public ViewResult Login(string returnUrl = "/")
        {
            return this.View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
            });
        }

        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (this.ModelState.IsValid)
            {
                IdentityUser user = await this.userManager.FindByNameAsync(loginViewModel.Name);

                if (user != null)
                {
                    await this.signInManager.SignOutAsync();

                    if ((await this.signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false)).Succeeded)
                    {
                        return this.RedirectToAction("Products", "Admin");
                    }
                }

                this.ModelState.AddModelError(string.Empty, "Invalid name or password.");
            }

            return this.View(loginViewModel);
        }

        [Route("Logout")]
        public async Task<IActionResult> Logout(string returnUrl = "/")
        {
            await this.signInManager.SignOutAsync();
            return this.RedirectToAction("Login", returnUrl);
        }
    }
}
