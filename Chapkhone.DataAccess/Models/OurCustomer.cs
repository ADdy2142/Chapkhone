using Chapkhone.DataAccess.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Models
{
    public class OurCustomer
    {
        [Key]
        public int Id { get; set; }

        public string LogoName { get; set; }

        public string LogoUrl => Urls.SiteUrl + "/images/" + LogoName;
    }
}