using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.ViewModels
{
    public class GetProductImageVM
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public bool ShowInSlider { get; set; }
    }

    public class AddProductImageVM
    {
        public int ProductId { get; set; }
        public ICollection<IFormFile> ProductImages { get; set; }
        public IFormFile SliderImage { get; set; }
    }
}