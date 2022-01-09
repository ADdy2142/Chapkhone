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

        [Required(ErrorMessage = "عنوان نباید خالی باشد.")]
        [MaxLength(50, ErrorMessage = "عنوان حداکثر می تواند 50 کاراکتر باشد.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "قیمت محصول را مشخص کنید.")]
        public double Price { get; set; }

        public double Discount { get; set; }

        [MaxLength(350, ErrorMessage = "توضیحات حداکثر می تواند 350 کاراکتر باشد.")]
        public double Description { get; set; }

        public int DesignGroupId { get; set; }

        [ForeignKey(nameof(DesignGroupId))]
        public DesignGroup DesignGroup { get; set; }

        public double FinalPrice => Price - (Price * Discount / 100);
        public ICollection<ProductImage> ProductImages { get; set; }
    }
}