using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Common;

namespace Ordering.Infrastructure.Configurations
{
    public class EntityBaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : EntityBase
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id);
            builder
                .Property(e => e.CreatedBy)
                .IsRequired()
                .HasMaxLength(200);

            builder
                .Property(e => e.CreatedDate)
                .IsRequired();
            
            builder
                .Property(e => e.LastModifiedBy)
                .IsRequired()
                .HasMaxLength(200);
            
            builder
                .Property(e => e.LastModifiedDate)
                .IsRequired();
        }
    }
}