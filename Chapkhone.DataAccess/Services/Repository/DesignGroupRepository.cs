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
    public class DesignGroupRepository : BaseRepository<DesignGroup>, IDesignGroupRepository
    {
        private readonly ChapkhoneContext _context;

        public DesignGroupRepository(ChapkhoneContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(DesignGroup designGroup)
        {
            var designGroupInDb = await FindAsync(designGroup.Id);
            if (designGroupInDb != null)
            {
                designGroupInDb.Title = designGroup.Title;
                designGroupInDb.ShortDescription = designGroup.ShortDescription;
                designGroupInDb.ImageName = designGroup.ImageName;
                designGroupInDb.UnitPrice = designGroup.UnitPrice;
                designGroupInDb.Discount = designGroup.Discount;
            }
        }
    }
}