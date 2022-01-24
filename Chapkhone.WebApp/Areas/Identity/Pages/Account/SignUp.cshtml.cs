using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Chapkhone.WebApp.Areas.Identity.Pages.Account
{
    public class SignUpModel : PageModel
    {
        private readonly SignInManager<Customer> _signInManager;
        private readonly UserManager<Customer> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<SignUpModel> _logger;

        [BindProperty]
        public SignUpVM SignUpVM { get; set; }

        public string ReturnUrl { get; set; }
        public string SignUpError { get; set; }

        public SignUpModel(SignInManager<Customer> signInManager, UserManager<Customer> userManager, RoleManager<IdentityRole> roleManager, ILogger<SignUpModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(SignUpVM.PhoneNumber);
                if (user != null)
                {
                    SignUpError = "این شماره تلفن قبلا استفاده شده است.";
                    return Page();
                }

                user = new Customer { UserName = SignUpVM.PhoneNumber, FirstName = SignUpVM.FirstName, LastName = SignUpVM.LastName, PhoneNumber = SignUpVM.PhoneNumber, ImageName = "default-user-image.png" };
                var result = await _userManager.CreateAsync(user, SignUpVM.Password);
                if (result.Succeeded)
                {
                    if (!await _roleManager.RoleExistsAsync("Admin"))
                        await _roleManager.CreateAsync(new IdentityRole("Admin"));

                    if (_userManager.Users.Count() == 1)
                        await _userManager.AddToRoleAsync(user, "Admin");

                    _logger.LogInformation("User created a new account with password.");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();
        }
    }
}