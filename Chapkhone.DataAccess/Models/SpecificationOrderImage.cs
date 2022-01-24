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
    public class SpecificationOrderImage
    {
        [Key]
        public int Id { get; set; }

        public string ImageName { get; set; }
        public int SpecificationOrderId { get; set; }

        [ForeignKey(nameof(SpecificationOrderId))]
        public SpecificationOrder SpecificationOrder { get; set; }

        public string ImageUrl => Urls.SiteUrl + "/images/" + ImageName;
    }
}