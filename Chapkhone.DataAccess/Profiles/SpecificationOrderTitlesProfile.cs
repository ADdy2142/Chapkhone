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
    public class SpecificationOrderTitlesProfile : Profile
    {
        public SpecificationOrderTitlesProfile()
        {
            CreateMap<SpecificationOrderTitle, SpecificationOrderTitleVM>().ForMember(dest => dest.Size, opt => opt.MapFrom(src => $"{src.DesktopSizeCssClass} {src.TabletSizeCssClass}"));
            CreateMap<SpecificationOrderTitle, GetSpecificationOrderTitleVM>();
            CreateMap<SpecificationOrderTitle, UpsertSpecificationOrderTitleVM>();
            CreateMap<UpsertSpecificationOrderTitleVM, SpecificationOrderTitle>();
        }
    }
}