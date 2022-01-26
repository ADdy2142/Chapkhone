using Chapkhone.DataAccess.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.ViewModels
{
    public class GetDesignGroupVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ProductsCount { get; set; }
    }

    public class UpsertDesignGroupVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "عنوان نباید خالی باشد.")]
        [MaxLength(50, ErrorMessage = "حداکثر طول عنوان باید 50 کاراکتر باشد.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "توضیحات کوتاه نباید خالی باشد.")]
        [MaxLength(100, ErrorMessage = "حداکثر طول توضیحات کوتاه باید 100 کاراکتر باشد.")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "هزینه طراحی نباید خالی باشد.")]
        public double UnitPrice { get; set; }

        [Required(ErrorMessage = "نحوه محاسبه قیمت را انتخاب کنید.")]
        public UnitPriceType UnitPriceType { get; set; }

        [Required(ErrorMessage = "تخفیف نباید خالی باشد.")]
        public double Discount { get; set; }

        public IFormFile DesignGroupImage { get; set; }
    }
}