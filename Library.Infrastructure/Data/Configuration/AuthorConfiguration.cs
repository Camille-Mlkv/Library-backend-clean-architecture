using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configuration
{
    public class AuthorConfiguration : BaseEntityConfiguration<Author>
    {
        protected override void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.Property(e => e.Name)
               .IsRequired()
               .HasMaxLength(100);

            builder.Property(e => e.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Country)
                   .HasMaxLength(50);

            builder.Property(e => e.BirthDay)
                   .IsRequired();

            builder.HasMany(e => e.Books)
                   .WithOne(b => b.Author)
                   .HasForeignKey(b => b.AuthorId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
