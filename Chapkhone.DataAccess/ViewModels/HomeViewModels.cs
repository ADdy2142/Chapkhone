using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.ViewModels
{
    public class IndexViewModel
    {
        public GetSiteNotificationViewModel SiteNotification { get; set; }
        private ICollection<GetProductViewModel> SlideShowProducts { get; set; }
        private ICollection<GetProductViewModel> NewProducts { get; set; }
        private ICollection<GetCustomerCommentViewModel> CustomerComments { get; set; }
    }
}