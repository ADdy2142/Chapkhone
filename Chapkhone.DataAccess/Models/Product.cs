using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public int Price { get; set; }
        public int Discount { get; set; }
        public string Description { get; set; }
        public int DesignGroupId { get; set; }

        [ForeignKey(nameof(DesignGroupId))]
        public DesignGroup DesignGroup { get; set; }

        public ICollection<ProductImage> ProductImages { get; set; }

        public int FinalPrice => Price - (Price * Discount / 100);
    }
}