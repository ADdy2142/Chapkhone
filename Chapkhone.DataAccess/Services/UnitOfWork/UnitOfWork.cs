using Chapkhone.DataAccess.Context;
using Chapkhone.DataAccess.Services.IRepository;
using Chapkhone.DataAccess.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Services.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ChapkhoneContext _context;

        public IDesignGroupRepository DesignGroups { get; private set; }
        public IProductRepository Products { get; private set; }
        public IProductImageRepository ProducImages { get; private set; }
        public ISpecificationOrderTypeRepository SpecificationOrderTypes { get; private set; }
        public ISpecificationOrderGroupRepository SpecificationOrderGroups { get; private set; }
        public ISpecificationOrderImageRepository SpecificationOrderImages { get; private set; }
        public ISpecificationOrderItemRepository SpecificationOrderItems { get; private set; }
        public ISpecificationOrderValueRepository SpecificationOrderValues { get; private set; }
        public ICustomerCommentRepository CustomerComments { get; private set; }
        public IOurCustomerRepository OurCustomers { get; private set; }

        public UnitOfWork(ChapkhoneContext context)
        {
            _context = context;
            DesignGroups = new DesignGroupRepository(_context);
            Products = new ProductRepository(_context);
            ProducImages = new ProductImageRepository(_context);
            SpecificationOrderTypes = new SpecificationOrderTypeRepository(_context);
            SpecificationOrderGroups = new SpecificationOrderGroupRepository(_context);
            SpecificationOrderImages = new SpecificationOrderImageRepository(_context);
            SpecificationOrderItems = new SpecificationOrderItemRepository(_context);
            SpecificationOrderValues = new SpecificationOrderValueRepository(_context);
            CustomerComments = new CustomerCommentRepository(_context);
            OurCustomers = new OurCustomerRepository(_context);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}