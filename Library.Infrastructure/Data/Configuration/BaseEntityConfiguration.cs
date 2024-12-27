using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure.Data.Configuration
{
    //public class BaseEntityConfiguration : IEntityTypeConfiguration<BaseEntity>
    //{
    //    public void Configure(EntityTypeBuilder<BaseEntity> builder)
    //    {
    //        builder.HasKey(e => e.Id);

    //        builder.Property(e => e.Id)
    //               .ValueGeneratedOnAdd();

    //    }
    //}
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        void IEntityTypeConfiguration<T>.Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

            Configure(builder);
        }

        protected abstract void Configure(EntityTypeBuilder<T> builder);
    }
}
