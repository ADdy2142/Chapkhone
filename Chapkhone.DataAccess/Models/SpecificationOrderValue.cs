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

        public string Value { get; set; }
        public int SpecificationOrderTitleId { get; set; }

        [ForeignKey(nameof(SpecificationOrderTitleId))]
        public SpecificationOrderTitle SpecificationOrderTitle { get; set; }

        public int SpecificationOrderId { get; set; }

        [ForeignKey(nameof(SpecificationOrderId))]
        public SpecificationOrder SpecificationOrder { get; set; }
    }
}