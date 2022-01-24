using Chapkhone.DataAccess.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chapkhone.DataAccess.Context
{
    public class ChapkhoneContext : IdentityDbContext<Customer>
    {
        public DbSet<CustomerComment> CustomerComments { get; set; }
        public DbSet<DesignGroup> DesignGroups { get; set; }
        public DbSet<OurCustomer> OurCustomers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<SiteNotification> SiteNotifications { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<SpecificationOrder> SpecificationOrders { get; set; }
        public DbSet<SpecificationOrderGroup> SpecificationOrderGroups { get; set; }
        public DbSet<SpecificationOrderImage> SpecificationOrderImages { get; set; }
        public DbSet<SpecificationOrderTitle> SpecificationOrderTitles { get; set; }
        public DbSet<SpecificationOrderValue> SpecificationOrderValues { get; set; }

        public ChapkhoneContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>()
                .HasOne(o => o.CustomerComment)
                .WithOne(cc => cc.Order)
                .HasForeignKey<CustomerComment>(cc => cc.OrderId);
        }
    }
}