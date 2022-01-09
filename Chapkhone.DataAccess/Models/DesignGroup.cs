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

        [Required(ErrorMessage = "عنوان نباید خالی باشد.")]
        [MaxLength(50, ErrorMessage = "عنوان حداکثر می تواند 50 کاراکتر باشد.")]
        public string Title { get; set; }

        public SpecificationOrderType SpecificationOrderType { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}