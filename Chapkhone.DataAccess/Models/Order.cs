using Chapkhone.DataAccess.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int TotalPrice { get; set; }
        public int TotalDiscount { get; set; }
        public DateTime? OrderDate { get; set; }
        public bool IsFinalized { get; set; }
        public bool TransactionStatus { get; set; }
        public bool IsVisitedByAdmin { get; set; }
        public string CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; }

        public CustomerComment CustomerComment { get; set; }
        public ICollection<SpecificationOrder> SpecificationOrders { get; set; }

        public int FinalPrice => TotalPrice - TotalDiscount;
        public string DefaultImageUrl => Urls.SiteUrl + "/images/" + "default-order-image.jpg";
    }
}