using Chapkhone.DataAccess.Services.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Services.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IDesignGroupRepository DesignGroups { get; }
        IProductRepository Products { get; }
        IProductImageRepository ProducImages { get; }
        ISpecificationOrderTypeRepository SpecificationOrderTypes { get; }
        ISpecificationOrderGroupRepository SpecificationOrderGroups { get; }
        ISpecificationOrderImageRepository SpecificationOrderImages { get; }
        ISpecificationOrderItemRepository SpecificationOrderItems { get; }
        ISpecificationOrderValueRepository SpecificationOrderValues { get; }
        ICustomerCommentRepository CustomerComments { get; }
        IOurCustomerRepository OurCustomers { get; }

        Task SaveAsync();
    }
}