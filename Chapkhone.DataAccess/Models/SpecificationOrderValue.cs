using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Models
{
    public class SpecificationOrderValue
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "مشتری گرامی، لطفا مقدار خواسته شده را وارد کنید.")]
        public string Value { get; set; }

        public int SpecificationOrderItemId { get; set; }

        [ForeignKey(nameof(SpecificationOrderItemId))]
        public SpecificationOrderItem SpecificationOrderItem { get; set; }
    }
}