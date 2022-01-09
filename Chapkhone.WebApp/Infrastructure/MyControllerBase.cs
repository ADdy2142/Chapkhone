using Chapkhone.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.WebApp.Infrastructure
{
    public class MyControllerBase : Controller
    {
        private readonly UserManager<Customer> _userManager;

        public string CurrentCustomerName
        {
            get
            {
                string userName = string.Empty;
                if (User.Identity.IsAuthenticated)
                    userName = User.Identity.Name;

                return userName;
            }
        }

        public Customer CurrentCustomer => GetCurrentCustomerAsync().Result;

        public MyControllerBase(UserManager<Customer> userManager)
        {
            _userManager = userManager;
        }

        private async Task<Customer> GetCurrentCustomerAsync()
        {
            return await _userManager.FindByNameAsync(CurrentCustomerName);
        }
    }
}