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
    public class SpecificationOrderItemRepository : BaseRepository<SpecificationOrderItem>, ISpecificationOrderItemRepository
    {
        private readonly ChapkhoneContext _context;

        public SpecificationOrderItemRepository(ChapkhoneContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(SpecificationOrderItem specificationOrderItem)
        {
            var specificationOrderItemInDb = await FindAsync(specificationOrderItem.Id);
            if (specificationOrderItemInDb != null)
            {
                specificationOrderItemInDb.Title = specificationOrderItem.Title;
                specificationOrderItemInDb.DesktopSize = specificationOrderItem.DesktopSize;
                specificationOrderItemInDb.TabletSize = specificationOrderItem.TabletSize;
            }
        }
    }
}