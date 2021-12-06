using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Configurations
{
    public class OrderConfiguration : EntityBaseConfiguration<Order>
    {
        public override void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("Orders", "Ordering");
            builder
                .Property(e => e.UserName)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(e => e.TotalPrice)
                .IsRequired()
                .HasPrecision(12, 2);

            builder
                .Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .Property(e => e.LastName)
                .HasMaxLength(100);
            builder
                .Property(e => e.EmailAddress)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .Property(e => e.AddressLine)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .Property(e => e.Country)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .Property(e => e.State)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .Property(e => e.ZipCode)
                .IsRequired()
                .HasMaxLength(100);

            builder
                .Property(e => e.CardName)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .Property(e => e.CardNumber)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .Property(e => e.Expiration)
                .IsRequired()
                .HasMaxLength(100);
            builder
                .Property(e => e.Cvv)
                .IsRequired()
                .HasMaxLength(3);

            builder
                .Property(e => e.PaymentMethod)
                .IsRequired();

            base.Configure(builder);
        }
    }
}