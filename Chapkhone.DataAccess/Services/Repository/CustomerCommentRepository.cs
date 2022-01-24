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
    public class CustomerCommentRepository : BaseRepository<CustomerComment>, ICustomerCommentRepository
    {
        private readonly ChapkhoneContext _context;

        public CustomerCommentRepository(ChapkhoneContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(CustomerComment customerComment)
        {
            var customerCommentInDb = await FindAsync(customerComment.Id);
            if (customerCommentInDb != null)
                customerCommentInDb.AllowedToShow = customerComment.AllowedToShow;
        }
    }
}