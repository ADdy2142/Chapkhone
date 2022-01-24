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
    public class SiteNotificationsProfile : Profile
    {
        public SiteNotificationsProfile()
        {
            CreateMap<SiteNotification, GetSiteNotificationVM>().ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrlXL));
            CreateMap<SiteNotification, UpsertSiteNotificationVM>();
            CreateMap<UpsertSiteNotificationVM, SiteNotification>();
        }
    }
}