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
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Product, GetProductViewModel>().ForMember(dest => dest.DesignGroupTitle, opt => opt.MapFrom(src => src.DesignGroup.Title));
        }
    }
}