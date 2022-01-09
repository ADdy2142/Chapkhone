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
    public class ProductImageRepository : BaseRepository<ProductImage>, IProductImageRepository
    {
        private readonly ChapkhoneContext _context;

        public ProductImageRepository(ChapkhoneContext context) : base(context)
        {
            _context = context;
        }
    }
}