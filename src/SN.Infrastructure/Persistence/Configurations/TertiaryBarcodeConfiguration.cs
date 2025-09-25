using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SN.Core.Domain.Barcodes;

namespace SN.Infrastructure.Persistence.Configurations
{
    public class TertiaryBarcodeConfiguration : IEntityTypeConfiguration<TertiaryBarcode>
    {
        public void Configure(EntityTypeBuilder<TertiaryBarcode> builder)
        {
            // Table name
            builder.ToTable("TertiaryBarcodes");
             builder.HasBaseType((Type?)null);
            // Primary key
            builder.HasKey(tb => tb.Id);

            // Properties
            builder.Property(tb => tb.BPOM2DBarCode)
                   .IsRequired()
                   .IsUnicode()
                   .HasMaxLength(100);

            builder.Property(tb => tb.DocumentId)
                   .IsRequired();

            builder.Property(tb => tb.RegistrationStatus)
                   .IsRequired();
                   
       // One-to-one: Tertiary -> Detail
              builder.HasOne(pb => pb.Detail)
                                   .WithOne()
                                   .HasForeignKey<TertiaryBarcode>(pb => pb.DetailId)
                                   .IsRequired()
                                   .OnDelete(DeleteBehavior.Restrict);
}
       }
}
