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
    public class SpecificationOrderValueRepository : BaseRepository<SpecificationOrderValue>, ISpecificationOrderValueRepository
    {
        private readonly ChapkhoneContext _context;

        public SpecificationOrderValueRepository(ChapkhoneContext context) : base(context)
        {
            _context = context;
        }

        public void DeleteRange(IEnumerable<SpecificationOrderValue> specificationOrderValues)
        {
            _context.SpecificationOrderValues.RemoveRange(specificationOrderValues);
        }
    }
}