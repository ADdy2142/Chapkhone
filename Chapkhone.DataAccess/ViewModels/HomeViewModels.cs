using Chapkhone.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.ViewModels
{
    public class MainPageVM
    {
        public SiteNotification SiteNotification { get; set; }
        public ICollection<ProductImage> SlideShowProducts { get; set; }
        public ICollection<Product> NewProducts { get; set; }
        public ICollection<CustomerComment> CustomerComments { get; set; }
        public ICollection<OurCustomer> OurCustomers { get; set; }
    }
}