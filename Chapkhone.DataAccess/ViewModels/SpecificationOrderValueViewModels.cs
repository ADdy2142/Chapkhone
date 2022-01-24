using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.ViewModels
{
    public class SpecificationOrderValueVM
    {
        [Required(ErrorMessage = "لطفا مقدار خواسته شده را وارد کنید.")]
        public string Value { get; set; }

        public int SpecificationOrderTitleId { get; set; }
    }
}