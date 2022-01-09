using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Models
{
    public class CustomerComment
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "لطفا نظر خود را وارد کنید.")]
        [MaxLength(350, ErrorMessage = "نظر شما بیش از حد طولانی است. حداکثر طول آن می تواند 350 کاراکتر باشد.")]
        public string CommentText { get; set; }

        public bool CanShow { get; set; }

        public string CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }
    }
}