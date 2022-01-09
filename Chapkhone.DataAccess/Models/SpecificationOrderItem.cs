using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Models
{
    public class SpecificationOrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "عنوان نباید خالی باشد.")]
        [MaxLength(50, ErrorMessage = "عنوان حداکثر می تواند 50 کاراکتر باشد.")]
        public string Title { get; set; }

        [MaxLength(100, ErrorMessage = "توضیحات کوتاه حداکثر می تواند 100 کاراکتر باشد.")]
        public string ShortDescription { get; set; }

        public SpecificationOrderItemSize DesktopSize { get; set; } = SpecificationOrderItemSize.Persent33;
        public SpecificationOrderItemSize TabletSize { get; set; } = SpecificationOrderItemSize.Persent50;
        public SpecificationOrderValue SpecificationOrderValue { get; set; }

        public int SpecificationOrderGroupId { get; set; }

        [ForeignKey(nameof(SpecificationOrderGroupId))]
        public SpecificationOrderGroup SpecificationOrderGroup { get; set; }
    }
}