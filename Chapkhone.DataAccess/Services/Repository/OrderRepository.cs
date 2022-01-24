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
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        private readonly ChapkhoneContext _context;

        public OrderRepository(ChapkhoneContext context) : base(context)
        {
            _context = context;
        }

        public async Task UpdateAsync(Order order)
        {
            var orderInDb = await FindAsync(order.Id);
            if (orderInDb != null)
            {
                orderInDb.TotalPrice = order.TotalPrice;
                orderInDb.TotalDiscount = order.TotalDiscount;
                orderInDb.OrderDate = order.OrderDate;
                orderInDb.IsFinalized = order.IsFinalized;
                orderInDb.TransactionStatus = order.TransactionStatus;
                orderInDb.IsVisitedByAdmin = order.IsVisitedByAdmin;
            }
        }
    }
}