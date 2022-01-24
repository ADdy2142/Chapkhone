using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Chapkhone.WebApp.Areas.Identity.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        private readonly UserManager<Customer> _userManager;
        private readonly SignInManager<Customer> _signInManager;

        [BindProperty]
        public ChangePasswordVM ChangePasswordVM { get; set; }

        public ChangePasswordModel(UserManager<Customer> userManager, SignInManager<Customer> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync(string email, string token)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
                return BadRequest();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound();

            ChangePasswordVM = new ChangePasswordVM { Token = token, Email = email };
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(ChangePasswordVM.Email);
                if (user == null)
                    return NotFound();

                var result = await _userManager.ResetPasswordAsync(user, ChangePasswordVM.Token, ChangePasswordVM.Password);
                if (result.Succeeded)
                    return RedirectToPage("./SignIn", new { changePassword = true });

                foreach (var error in result.Errors)
                    ModelState.TryAddModelError(error.Code, error.Description);
            }

            return Page();
        }
    }
}