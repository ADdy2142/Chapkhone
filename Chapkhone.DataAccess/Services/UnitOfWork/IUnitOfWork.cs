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
        IOrderRepository Orders { get; }
        ISpecificationOrderRepository SpecificationOrders { get; }
        ISpecificationOrderGroupRepository SpecificationOrderGroups { get; }
        ISpecificationOrderImageRepository SpecificationOrderImages { get; }
        ISpecificationOrderTitleRepository SpecificationOrderTitles { get; }
        ISpecificationOrderValueRepository SpecificationOrderValues { get; }
        ICustomerCommentRepository CustomerComments { get; }
        IOurCustomerRepository OurCustomers { get; }
        ISiteNotificationRepository SiteNotifications { get; }

        Task SaveAsync();
    }
}