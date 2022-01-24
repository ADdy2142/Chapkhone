using Chapkhone.DataAccess.Constants;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Models
{
    public class Customer : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ImageName { get; set; }

        public ICollection<Order> Orders { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public string ImageUrl => Urls.SiteUrl + "/images/" + ImageName;
    }
}