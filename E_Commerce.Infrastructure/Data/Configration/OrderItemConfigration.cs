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
    internal class OrderItemConfigration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {

            builder.Property(o => o.Price).HasColumnType("decimal(8,2)");
            builder.OwnsOne(o => o.Product, p =>
            {

                p.Property(p => p.ProductName).HasMaxLength(100);
                p.Property(p => p.PictureUrl).HasMaxLength(200);
            });
                }
    }
}
