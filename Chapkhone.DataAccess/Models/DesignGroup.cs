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
    public class DesignGroup
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string ImageName { get; set; }
        public int DesignPrice { get; set; }
        public int Discount { get; set; }

        public ICollection<Product> Products { get; set; }
        public ICollection<SpecificationOrderGroup> SpecificationOrderGroups { get; set; }

        public string ImageUrl => Urls.SiteUrl + "/images/" + ImageName;
        public int FinalPrice => DesignPrice - (DesignPrice * Discount / 100);
    }
}