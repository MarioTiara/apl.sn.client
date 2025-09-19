using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SN.Core.Domain.Barcodes;

namespace SN.Infrastructure.Persistence.Configurations
{
    public class BarcodeConfiguration : IEntityTypeConfiguration<Barcode>
    {
        public void Configure(EntityTypeBuilder<Barcode> builder)
        {
            // Table name
            builder.ToTable("Barcodes");

            // Primary key
            builder.HasKey(b => b.Id);

            // Properties
            builder.Property(b => b.BPOM2DBarCode)
                   .IsRequired()
                   .HasMaxLength(255);

            builder.Property(b => b.Gtin)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(b => b.Serial)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(b => b.Batch)
                   .HasMaxLength(50);

            builder.Property(b => b.ExpireDate)
                   .HasColumnType("date"); // DateOnly mapping

            // Indexes (optional)
            builder.HasIndex(b => b.BPOM2DBarCode)
                   .IsUnique(); // Unique constraint on barcode value
        }
    }
}
