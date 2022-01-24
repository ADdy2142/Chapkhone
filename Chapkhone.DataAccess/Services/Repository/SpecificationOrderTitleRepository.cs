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
    public class SpecificationOrderTitleRepository : BaseRepository<SpecificationOrderTitle>, ISpecificationOrderTitleRepository
    {
        private readonly ChapkhoneContext _context;

        public SpecificationOrderTitleRepository(ChapkhoneContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(SpecificationOrderTitle specificationOrderItem)
        {
            var specificationOrderItemInDb = await FindAsync(specificationOrderItem.Id);
            if (specificationOrderItemInDb != null)
            {
                specificationOrderItemInDb.Title = specificationOrderItem.Title;
                specificationOrderItemInDb.ShortDescription = specificationOrderItem.ShortDescription;
                specificationOrderItemInDb.DesktopSize = specificationOrderItem.DesktopSize;
                specificationOrderItemInDb.TabletSize = specificationOrderItem.TabletSize;
            }
        }
    }
}