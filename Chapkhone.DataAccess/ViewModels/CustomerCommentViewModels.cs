using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.ViewModels
{
    public class GetCustomerCommentViewModel
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
        public string CustomerImage { get; set; }
        public string CustomerName { get; set; }
    }
}