using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SN.Core.Domain.Barcodes;

public class SecondaryBarcodeConfiguration : IEntityTypeConfiguration<SecondaryBarcode>
{
       public void Configure(EntityTypeBuilder<SecondaryBarcode> builder)
       {
              builder.ToTable("SecondaryBarcodes");
              builder.HasBaseType((Type?)null);
              builder.HasKey(sb => sb.Id);

              builder.Property(sb => sb.BPOM2DBarCode)
                     .IsRequired()
                     .HasMaxLength(100);

              builder.Property(sb => sb.DocumentId)
                     .IsRequired();

              builder.Property(sb => sb.RegistrationStatus)
                     .HasConversion<string>()
               .IsRequired();

              // One-to-one: Secondary -> Detail
              builder.HasOne(pb => pb.Detail)
                                   .WithOne()
                                   .HasForeignKey<SecondaryBarcode>(pb => pb.DetailId)
                                   .IsRequired()
                                   .OnDelete(DeleteBehavior.Restrict);

              // Many-to-one: Secondary -> Tertiary
              builder.HasOne(sb => sb.Parent)
                     .WithMany(tb => tb.Secondaries)
                     .HasForeignKey(sb => sb.ParentId)
                     .IsRequired(false)
                   .HasPrincipalKey(tb => tb.Id)
               .OnDelete(DeleteBehavior.Restrict);

              // One-to-many: Secondary -> Primary
              builder.HasMany(sb => sb.Primaries)
                     .WithOne(p => p.Parent)
                     .HasForeignKey(p => p.ParentId)
                     .HasPrincipalKey(sb => sb.Id)
                     .OnDelete(DeleteBehavior.Restrict);
       }
}
