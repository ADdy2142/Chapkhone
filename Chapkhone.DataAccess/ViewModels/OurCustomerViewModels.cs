using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.ViewModels
{
    public class GetOurCustomerVM
    {
        public int Id { get; set; }
        public string LogoUrl { get; set; }
    }

    public class UpsertOurCustomerVM
    {
        public int Id { get; set; }
        public IFormFile Logo { get; set; }
    }
}