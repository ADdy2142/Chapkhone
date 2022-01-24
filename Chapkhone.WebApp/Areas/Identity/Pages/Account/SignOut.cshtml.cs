using Chapkhone.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Chapkhone.WebApp.Areas.Identity.Pages.Account
{
    public class SignOutModel : PageModel
    {
        private SignInManager<Customer> _signInManager;

        public SignOutModel(SignInManager<Customer> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> OnGetAsync(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            return LocalRedirect(returnUrl);
        }
    }
}