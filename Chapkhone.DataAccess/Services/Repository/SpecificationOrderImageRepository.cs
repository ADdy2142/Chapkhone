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
    public class SpecificationOrderImageRepository : BaseRepository<SpecificationOrderImage>, ISpecificationOrderImageRepository
    {
        private readonly ChapkhoneContext _context;

        public SpecificationOrderImageRepository(ChapkhoneContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<SpecificationOrderImage> specificationOrderImages)
        {
            await _context.SpecificationOrderImages.AddRangeAsync(specificationOrderImages);
        }

        public void DeleteRange(IEnumerable<SpecificationOrderImage> specificationOrderImages)
        {
            _context.SpecificationOrderImages.RemoveRange(specificationOrderImages);
        }
    }
}