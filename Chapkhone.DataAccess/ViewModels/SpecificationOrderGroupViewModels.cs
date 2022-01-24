using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.ViewModels
{
    public class SpecificationOrderGroupVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<SpecificationOrderTitleVM> SpecificationOrderTitles { get; set; }
    }

    public class GetSpecificationOrderGroupVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class UpsertSpecificationOrderGroupVM
    {
        public int Id { get; set; }
        public int DesignGroupId { get; set; }

        [Required(ErrorMessage = "عنوان نباید خالی باشد.")]
        [MaxLength(50, ErrorMessage = "حداکثر طول عنوان می تواند 50 کاراکتر باشد. ")]
        public string Title { get; set; }
    }
}