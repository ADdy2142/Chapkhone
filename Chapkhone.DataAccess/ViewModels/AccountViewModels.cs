using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.ViewModels
{
    public class SignUpVM
    {
        [Required(ErrorMessage = "لطفا نام خود را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "نام شما بیش از حد طولانی است.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "لطفا نام خانوادگی خود را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "نام خانوادگی شما بیش از حد طولانی است.")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "لطفا شماره تلفن خود را وارد کنید.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "لطفا شماره تلفن معتبر وارد کنید.")]
        [RegularExpression("09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}", ErrorMessage = "شماره تلفن شما معتبر نمی باشد.")]
        [MaxLength(11, ErrorMessage = "شماره تلفن شما معتبر نمی باشد.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفا کلمه عبور خود را وارد کنید.")]
        [MinLength(8, ErrorMessage = "کلمه عبور حداقل باید 8 کاراکتر باشد.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "کلمه عبور و تکرار آن یکسان نیستند.")]
        public string ConfirmPassword { get; set; }
    }

    public class SignInVM
    {
        [Required(ErrorMessage = "لطفا شماره تلفن خود را وارد کنید.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "لطفا شماره تلفن معتبر وارد کنید.")]
        [RegularExpression("09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}", ErrorMessage = "شماره تلفن شما معتبر نمی باشد.")]
        [MaxLength(11, ErrorMessage = "شماره تلفن شما معتبر نمی باشد.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفا کلمه عبور خود را وارد کنید.")]
        [MinLength(8, ErrorMessage = "کلمه عبور حداقل باید 8 کاراکتر باشد.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class CustomerDetailsVM
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
    }

    public class EditCustomerPersonalInfoVM
    {
        [Required(ErrorMessage = "لطفا نام خود را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "نام شما بیش از حد طولانی است.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "لطفا نام خانوادگی خود را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "نام خانوادگی شما بیش از حد طولانی است.")]
        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده صحیح نمی باشد.")]
        public string Email { get; set; }
    }

    public class ChangeCustomerPasswordVM
    {
        [Required(ErrorMessage = "لطفا کلمه عبور فعلی خود را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "کلمه عبور شما بیش از حد طولانی است.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "لطفا کلمه عبور جدید خود را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "کلمه عبور شما بیش از حد طولانی است.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare(nameof(NewPassword), ErrorMessage = "کلمه عبور جدید شما با تکرار آن یکسان نیستند.")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }

    public class ForgetPasswordVM
    {
        [Required(ErrorMessage = "لطفا شماره تلفن خود را وارد کنید.")]
        [DataType(DataType.PhoneNumber, ErrorMessage = "لطفا شماره تلفن معتبر وارد کنید.")]
        [RegularExpression("09(1[0-9]|3[1-9]|2[1-9])-?[0-9]{3}-?[0-9]{4}", ErrorMessage = "شماره تلفن شما معتبر نمی باشد.")]
        [MaxLength(11, ErrorMessage = "شماره تلفن شما معتبر نمی باشد.")]
        public string PhoneNumber { get; set; }
    }

    public class ChangePasswordVM
    {
        [Required(ErrorMessage = "مشخصات شما کامل نیست.")]
        public string Token { get; set; }

        [Required(ErrorMessage = "مشخصات شما کامل نیست.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا کلمه عبور جدید خود را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "کلمه عبور شما بیش از حد طولانی است.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "کلمه عبور جدید شما با تکرار آن یکسان نیستند.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}