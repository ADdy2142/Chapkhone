using AutoMapper;
using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Profiles
{
    public class ProductImagesProfile : Profile
    {
        public ProductImagesProfile()
        {
            CreateMap<ProductImage, GetProductImageVM>();
            CreateMap<AddProductImageVM, ProductImage>();
        }
    }
}