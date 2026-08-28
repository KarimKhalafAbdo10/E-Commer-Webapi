using E_Commerce.Domain.Entities.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Data.Configration
{
    internal class OrderConfigrations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.HasMany(o => o.Items)
                .WithOne().OnDelete(DeleteBehavior.Cascade);
            builder.Property(o=>o.SubTotal).HasColumnType("decimal(8,2)");
            builder.OwnsOne(o => o.ShippingAddress,address=>
            {
                address.Property(x => x.FirstName).HasMaxLength(50);
                address.Property(x => x.LastName).HasMaxLength(50);
                address.Property(x => x.Street).HasMaxLength(50);
                address.Property(x => x.City).HasMaxLength(50);
                address.Property(x => x.Country).HasMaxLength(50);


            });


            builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(50);
        }
    }
}
