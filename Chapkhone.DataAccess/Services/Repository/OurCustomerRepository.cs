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
    public class OurCustomerRepository : BaseRepository<OurCustomer>, IOurCustomerRepository
    {
        private readonly ChapkhoneContext _context;

        public OurCustomerRepository(ChapkhoneContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(OurCustomer ourCustomer)
        {
            var ourCustomerInDb = await FindAsync(ourCustomer.Id);
            if (ourCustomerInDb != null)
                ourCustomerInDb.Logo = ourCustomer.Logo;
        }
    }
}