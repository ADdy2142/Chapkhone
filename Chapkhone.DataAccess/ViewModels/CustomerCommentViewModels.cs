using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.ViewModels
{
    public class GetCustomerCommentVM
    {
        public int Id { get; set; }
        public string CommentText { get; set; }
    }

    public class UpsertCustomerCommentVM
    {
        public bool AllowedToShow { get; set; }

        [Required(ErrorMessage = "لطفا نظر خود را وارد کنید.")]
        [MaxLength(500, ErrorMessage = "نظر شما بیش از حد طولانی است.")]
        public string CommentText { get; set; }

        public int OrderId { get; set; }
    }
}