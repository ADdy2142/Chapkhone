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
    public class SpecificationOrderTypeRepository : BaseRepository<SpecificationOrderType>, ISpecificationOrderTypeRepository
    {
        private readonly ChapkhoneContext _context;

        public SpecificationOrderTypeRepository(ChapkhoneContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(SpecificationOrderType specificationOrderType)
        {
            var specificationOrderTypeInDb = await FindAsync(specificationOrderType.Id);
            if (specificationOrderTypeInDb != null)
            {
                specificationOrderTypeInDb.Title = specificationOrderType.Title;
                specificationOrderTypeInDb.ShortDescription = specificationOrderType.ShortDescription;
                specificationOrderTypeInDb.BannerUrl = specificationOrderType.BannerUrl;
                specificationOrderTypeInDb.Price = specificationOrderType.Price;
            }
        }
    }
}