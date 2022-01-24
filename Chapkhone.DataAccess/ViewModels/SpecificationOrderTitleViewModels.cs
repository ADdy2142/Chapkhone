using Chapkhone.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.ViewModels
{
    public class SpecificationOrderTitleVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Size { get; set; }
        public string ShortDescription { get; set; }
    }

    public class GetSpecificationOrderTitleVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class UpsertSpecificationOrderTitleVM
    {
        public int Id { get; set; }
        public int SpecificationOrderGroupId { get; set; }

        [Required(ErrorMessage = "عنوان نباید خالی باشد.")]
        [MaxLength(50, ErrorMessage = "عنوان حداکثر می تواند 50 کاراکتر باشد.")]
        public string Title { get; set; }

        [MaxLength(100, ErrorMessage = "توضیحات کوتاه حداکثر می تواند 100 کاراکتر باشد.")]
        public string ShortDescription { get; set; }

        public SpecificationOrderTitleSize DesktopSize { get; set; }
        public SpecificationOrderTitleSize TabletSize { get; set; }
    }
}