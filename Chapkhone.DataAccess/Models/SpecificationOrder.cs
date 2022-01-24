using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Models
{
    public class SpecificationOrder
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public int Qty { get; set; }
        public string Description { get; set; }
        public int TotalPrice { get; set; }
        public int TotalDiscount { get; set; }
        public int OrderId { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }

        public int ProductId { get; set; }
        public int DesignGroupId { get; set; }

        public ICollection<SpecificationOrderValue> SpecificationOrderValues { get; set; }
        public ICollection<SpecificationOrderImage> SpecificationOrderImages { get; set; }

        public int FinalPrice => TotalPrice - TotalDiscount;
    }
}