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
    public class OurCustomersProfile : Profile
    {
        public OurCustomersProfile()
        {
            CreateMap<OurCustomer, GetOurCustomerVM>();
            CreateMap<OurCustomer, UpsertOurCustomerVM>();
            CreateMap<UpsertOurCustomerVM, OurCustomer>();
        }
    }
}