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
    public class CustomersProfile : Profile
    {
        public CustomersProfile()
        {
            CreateMap<Customer, CustomerDetailsVM>();
            CreateMap<Customer, EditCustomerPersonalInfoVM>();
            CreateMap<EditCustomerPersonalInfoVM, Customer>();
        }
    }
}