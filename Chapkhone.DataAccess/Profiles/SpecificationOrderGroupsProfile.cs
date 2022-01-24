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
    public class SpecificationOrderGroupsProfile : Profile
    {
        public SpecificationOrderGroupsProfile()
        {
            CreateMap<SpecificationOrderGroup, SpecificationOrderGroupVM>();
            CreateMap<SpecificationOrderGroup, GetSpecificationOrderGroupVM>();
            CreateMap<SpecificationOrderGroup, UpsertSpecificationOrderGroupVM>().ForMember(dest => dest.DesignGroupId, opt => opt.MapFrom(src => src.DesignGroupId));
            CreateMap<UpsertSpecificationOrderGroupVM, SpecificationOrderGroup>();
        }
    }
}