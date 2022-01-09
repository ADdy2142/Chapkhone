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
        public DbSet<SpecificationOrderGroup> SpecificationOrderGroups { get; set; }
        public DbSet<SpecificationOrderImage> SpecificationOrderImages { get; set; }
        public DbSet<SpecificationOrderItem> SpecificationOrderItems { get; set; }
        public DbSet<SpecificationOrderType> SpecificationOrderTypes { get; set; }
        public DbSet<SpecificationOrderValue> SpecificationOrderValues { get; set; }

        public ChapkhoneContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<DesignGroup>()
                .HasOne(dg => dg.SpecificationOrderType)
                .WithOne(spec => spec.DesignGroup)
                .HasForeignKey<SpecificationOrderType>(spec => spec.DesignGroupId);

            builder.Entity<SpecificationOrderItem>()
                .HasOne(spec => spec.SpecificationOrderValue)
                .WithOne(spec => spec.SpecificationOrderItem)
                .HasForeignKey<SpecificationOrderValue>(spec => spec.SpecificationOrderItemId);
        }
    }
}