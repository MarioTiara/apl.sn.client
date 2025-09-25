using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SN.Core.Domain.Barcodes;

namespace SN.Infrastructure.Persistence.Configurations
{
       public class PrimaryBarcodeConfiguration : IEntityTypeConfiguration<PrimaryBarcode>
       {
              public void Configure(EntityTypeBuilder<PrimaryBarcode> builder)
              {
                     // Table name
                     builder.ToTable("PrimaryBarcodes");
                     builder.HasBaseType((Type?)null);
                     // Primary key
                     builder.HasKey(pb => pb.Id);

                     // Properties
                     builder.Property(pb => pb.BPOM2DBarCode)
                            .IsRequired()
                            .HasMaxLength(100);

                     builder.Property(pb => pb.DocumentId)
                            .IsRequired();

                     builder.Property(pb => pb.RegistrationStatus)
                            .IsRequired();

                     builder.Property(pb => pb.CreatedAt)
                            .IsRequired();

                     // One-to-one: Primary -> Detail
                     builder.HasOne(pb => pb.Detail)
                                   .WithOne()
                                   .HasForeignKey<PrimaryBarcode>(pb => pb.DetailId)
                                   .IsRequired()
                                   .OnDelete(DeleteBehavior.Restrict);

                     // Many-to-one: Primary -> Secondary
                     builder.HasOne(pb => pb.Parent)
                            .WithMany(sb => sb.Primaries)
                            .HasForeignKey(pb => pb.ParentId)
                            .HasPrincipalKey(sb => sb.Id) // Optional if Id is PK
                            .OnDelete(DeleteBehavior.Restrict);
              }
       }
}
