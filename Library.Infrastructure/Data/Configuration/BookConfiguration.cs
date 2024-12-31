using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Library.Infrastructure.Data.Configuration
{
    public class BookConfiguration : BaseEntityConfiguration<Book>
    {
        protected override void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.Property(e => e.ISBN)
               .IsRequired()
               .HasMaxLength(50);

            builder.Property(e => e.Title)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(e => e.Genre)
                   .HasMaxLength(50);

            builder.Property(e => e.Description)
                   .HasMaxLength(1000);

            builder.Property(e => e.ImagePath)
                   .HasMaxLength(1000);

            builder.HasOne(e => e.Author)
                   .WithMany(a => a.Books)
                   .HasForeignKey(e => e.AuthorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(e => e.TakenTime)
                   .IsRequired(false);

            builder.Property(e => e.ReturnBy)
                   .IsRequired(false);

            builder.Property(e => e.ClientId)
                   .IsRequired(false);
        }
    }
}
