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
    public class DesignGroupsProfile : Profile
    {
        public DesignGroupsProfile()
        {
            CreateMap<DesignGroup, GetDesignGroupVM>().ForMember(dest => dest.ProductsCount, opt => opt.MapFrom(src => src.Products.Count));
            CreateMap<DesignGroup, UpsertDesignGroupVM>();
            CreateMap<UpsertDesignGroupVM, DesignGroup>();
        }
    }
}