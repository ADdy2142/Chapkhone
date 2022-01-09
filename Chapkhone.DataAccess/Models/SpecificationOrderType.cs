using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Models
{
    public class SpecificationOrderType
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "عنوان نباید خالی باشد.")]
        [MaxLength(50, ErrorMessage = "عنوان حداکثر می تواند 50 کاراکتر باشد.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "توضیحات کوتاه نباید خالی باشد.")]
        [MaxLength(100, ErrorMessage = "توضیحات کوتاه حداکثر می تواند 100 کاراکتر باشد.")]
        public string ShortDescription { get; set; }

        public string BannerUrl { get; set; }

        [Required(ErrorMessage = "یک قیمت ثابت برای این نوع طراحی مشخص کنید.")]
        public double Price { get; set; }

        [Required(ErrorMessage = "مشتری گرامی، عنوان نباید خالی باشد.")]
        [MaxLength(50, ErrorMessage = "مشتری گرامی، عنوان حداکثر می تواند 50 کاراکتر باشد.")]
        public string OrderTitle { get; set; }

        [Required(ErrorMessage = "مشتری گرامی، لطفا تعداد سفارش خود را تعیین کنید.")]
        public int OrderQty { get; set; }

        [MaxLength(350, ErrorMessage = "مشتری گرامی، توضیحات شما بیش از حد طولانی است. تنها 350 کاراکتر قابل قبول است.")]
        public string OrderDescription { get; set; }

        public int DesignGroupId { get; set; }

        [ForeignKey(nameof(DesignGroupId))]
        public DesignGroup DesignGroup { get; set; }

        public ICollection<SpecificationOrderImage> SpecificationOrderImages { get; set; }
        public ICollection<SpecificationOrderGroup> SpecificationOrderGroups { get; set; }
    }
}