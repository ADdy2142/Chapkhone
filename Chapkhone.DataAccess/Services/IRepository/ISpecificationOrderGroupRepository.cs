using Chapkhone.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Services.IRepository
{
    public interface ISpecificationOrderGroupRepository : IBaseRepository<SpecificationOrderGroup>
    {
        Task UpdateAsync(SpecificationOrderGroup specificationOrderGroup);
    }
}