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
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        private readonly ChapkhoneContext _context;

        public ProductRepository(ChapkhoneContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(Product product)
        {
            var productInDb = await FindAsync(product.Id);
            if (productInDb != null)
            {
                productInDb.Title = product.Title;
                productInDb.Price = product.Price;
                productInDb.Description = product.Description;
                productInDb.Discount = product.Discount;
                productInDb.DesignGroupId = product.DesignGroupId;
            }
        }
    }
}