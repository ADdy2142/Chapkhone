using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Chapkhone.WebApp.Areas.Identity.Pages.Account
{
    public class SignInModel : PageModel
    {
        private readonly SignInManager<Customer> _signInManager;
        private readonly ILogger<SignInModel> _logger;

        [BindProperty]
        public SignInVM SignInVM { get; set; }

        public string ReturnUrl { get; set; }
        public string SignInError { get; set; }
        public bool ForgetPassword { get; set; }
        public bool ChangePassword { get; set; }

        public SignInModel(SignInManager<Customer> signInManager, ILogger<SignInModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public void OnGet(string returnUrl = null, bool forgetPassword = false, bool changePassword = false)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ReturnUrl = returnUrl;
            ForgetPassword = forgetPassword;
            ChangePassword = changePassword;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(SignInVM.PhoneNumber,
                    SignInVM.Password, SignInVM.RememberMe, lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return LocalRedirect(returnUrl);
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    //ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    SignInError = "نام کاربری یا کلمه عبور شما اشتباه است.";
                    return Page();
                }
            }

            return Page();
        }
    }
}