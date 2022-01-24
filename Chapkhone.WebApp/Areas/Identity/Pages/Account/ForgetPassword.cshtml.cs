using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.ViewModels;
using Chapkhone.Utilities.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace Chapkhone.WebApp.Areas.Identity.Pages.Account
{
    public class ForgetPasswordModel : PageModel
    {
        private readonly UserManager<Customer> _userManager;
        private readonly IMailService _mailService;

        [BindProperty]
        public ForgetPasswordVM ForgetPasswordVM { get; set; }

        public ForgetPasswordModel(UserManager<Customer> userManager, IMailService mailService)
        {
            _userManager = userManager;
            _mailService = mailService;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(ForgetPasswordVM.PhoneNumber);
                if (user != null && !string.IsNullOrEmpty(user.Email))
                {
                    var tokent = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var changePasswordUrl = Url.Action("ChangePassword", "Account", new { area = "Identity", token = tokent, email = user.Email }, Request.Scheme);
                    var mailRequest = new MailRequestVM
                    {
                        ToEmail = user.Email,
                        Attachments = null,
                        Body = @"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <meta http-equiv='X-UA-Compatible' content='ie=edge'>
    <meta http-equiv='Content-Type' content='text/html;charset=UTF-8'>
    <title>فراموشی کلمه عبور</title>
    <style type='text/css' media='screen'>
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            font-family: 'Tahoma', sans-serif;
            color: #222;
        }

        body {
            padding: 25px 20px;
            background-color: #FFF;
        }

        .email-title {
            display: block;
            text-align: center;
            padding: 20px;
            border-radius: 5px;
            background-color: #FFC107;
            font-size: 18px;
            font-weight: bold;
        }

        .email-text {
            display: block;
            text-align: center;
            padding: 12px 8px;
            margin: 10px 0;
        }

        .email-button-wrap {
            display: block;
            width: 100 %;
            text-align: center;
        }

        .email-button-wrap .email-button {
            display: inline-block;
            padding: 8px 12px;
            background-color: #FFC107;
            border-radius: 5px;
            box-shadow: 0 3px 5px rgba(0, 0, 0, 0.3);
            text-decoration: none;
            font-size: 12px;
        }

        footer {
            margin-top: 20px;
            background-color: #222;
            border-radius: 5px;
        }

        footer.email-text {
            color: #FFF;
        }
    </style>
</head>
<body dir='rtl'>
<h1 class='email-title'>فراموشی کلمه عبور</h1>
<span class='email-text'>[username] عزیز برای تغییر کلمه عبور خود روی لینک زیر کلیک کنید.</span>
<div class='email-button-wrap'>
    <a href='[changePasswordUrl]' class='email-button'>تغییر کلمه عبور</a>
</div>
<footer>
    <span class='email-text'>چاپخانه بخشی</span>
</footer>
</body>
</html>
",
                        Subject = "فراموشی کلمه عبور"
                    };

                    mailRequest.Body = mailRequest.Body.Replace("[username]", user.FullName).Replace("[changePasswordUrl]", changePasswordUrl);

                    await _mailService.SendEmailAsync(mailRequest);
                }

                return RedirectToPage("./SignIn", new { forgetPassword = true });
            }

            return Page();
        }
    }
}