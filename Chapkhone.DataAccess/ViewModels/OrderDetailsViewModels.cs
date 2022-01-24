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
    public class GetOrderDetailsVM
    {
        public int DesignGroupId { get; set; }
        public int ProductId { get; set; }
        public string DesignGroupImageUrl { get; set; }

        [Required(ErrorMessage = "لطفا عنوان را وارد کنید.")]
        [MaxLength(50, ErrorMessage = "مشتری گرامی، عنوان وارد شده بیش از حد طولانی است.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "لطفا تعداد سفارش را وارد کنید.")]
        public int Qty { get; set; }

        [MaxLength(1500, ErrorMessage = "مشتری گرامی، توضیحات شما بیش از حد طولانی است.")]
        public string Description { get; set; }

        public ICollection<SpecificationOrderGroupVM> SpecificationOrderGroups { get; set; }
        public SpecificationOrderValueVM[] SpecificationOrderValues { get; set; }
        public List<IFormFile> CustomerImages { get; set; }
    }
}