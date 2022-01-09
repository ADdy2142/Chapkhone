using Chapkhone.DataAccess.Context;
using Chapkhone.DataAccess.Models;
using Chapkhone.DataAccess.Services.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Services.Repository
{
    public class SpecificationOrderGroupRepository : BaseRepository<SpecificationOrderGroup>, ISpecificationOrderGroupRepository
    {
        private readonly ChapkhoneContext _context;

        public SpecificationOrderGroupRepository(ChapkhoneContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(SpecificationOrderGroup specificationOrderGroup)
        {
            var specificationOrderGroupInDb = await FindAsync(specificationOrderGroup.Id);
            if (specificationOrderGroupInDb != null)
                specificationOrderGroupInDb.Title = specificationOrderGroup.Title;
        }
    }
}