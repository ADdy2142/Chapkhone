﻿using Chapkhone.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Services.IRepository
{
    public interface IOurCustomerRepository : IBaseRepository<OurCustomer>
    {
        Task UpdateAsync(OurCustomer ourCustomer);
    }
}