using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.ViewModels
{
    public class GetProductVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double FinalPrice { get; set; }
        public string DesignGroupTitle { get; set; }
        public ICollection<GetProductImageVM> ProductImages { get; set; }
    }

    public class UpsertProductVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "عنوان نباید خالی باشد.")]
        [MaxLength(50, ErrorMessage = "عنوان حداکثر می تواند 50 کاراکتر باشد.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "قیمت محصول را مشخص کنید.")]
        public double Price { get; set; }

        public double Discount { get; set; }

        [MaxLength(350, ErrorMessage = "توضیحات حداکثر می تواند 350 کاراکتر باشد.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "لطفا یک گروه طراحی انتخاب کنید.")]
        public int DesignGroupId { get; set; }

        public ICollection<IFormFile> ProductImageFiles { get; set; }
        public IFormFile SliderImageFile { get; set; }
    }

    public class GeneralDiscountVM
    {
        [Required(ErrorMessage = "لطفا درصد تخفیف را مشخص کنید.")]
        [Range(0, 100, ErrorMessage = "درصد تخفیف باید عددی بین 0 تا 100 باشد.")]
        public int Discount { get; set; }
    }
}