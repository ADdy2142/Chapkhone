using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Models
{
    public class SpecificationOrderImage
    {
        [Key]
        public int Id { get; set; }

        public string Url { get; set; }
        public int SpecificationOrderTypeId { get; set; }

        [ForeignKey(nameof(SpecificationOrderTypeId))]
        public SpecificationOrderType SpecificationOrderType { get; set; }
    }
}