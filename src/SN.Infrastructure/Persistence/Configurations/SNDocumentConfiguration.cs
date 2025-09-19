using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SN.Core.Domain.Barcodes;
using SN.Core.Domain.Documents;

namespace SN.Infrastructure.Persistence.Configurations
{
       public class SNDocumentConfiguration : IEntityTypeConfiguration<SNDocument>
       {
              public void Configure(EntityTypeBuilder<SNDocument> builder)
              {
                     // Table name
                     builder.ToTable("SNDocuments");

                     // Primary key
                     builder.HasKey(d => d.Id);

                     // Properties
                     builder.Property(d => d.FileName).HasMaxLength(255);
                     builder.Property(d => d.FileExtension).HasMaxLength(10);
                     builder.Property(d => d.DocumentIdentifier).HasMaxLength(255);
                     builder.Property(d => d.TransactionCode).HasMaxLength(50);
                     builder.Property(d => d.SenderIdentifier).IsRequired().HasMaxLength(50);
                     builder.Property(d => d.ReceiverIdentifier).IsRequired().HasMaxLength(50);
                     builder.Property(d => d.CreatedAt).IsRequired();

                     // Relationship: SNDocument -> Producer (Company)
                     builder.HasOne(d => d.Producer)
                            .WithMany()
                            .IsRequired()
                            .OnDelete(DeleteBehavior.Restrict);

                     // Relationship: SNDocument -> EpcisNodes
                     builder.HasMany(d => d.EpcisNodes)
                            .WithOne()
                            .HasForeignKey(e => e.DocumentId)
                            .OnDelete(DeleteBehavior.Cascade);

                     // Ignore the private Barcodes collection for EF mapping
                     builder.Ignore(d => d.Barcodes);

                     builder.HasMany<PrimaryBarcode>()
                .WithOne()
                .HasForeignKey(b => b.DocumentId)
                .OnDelete(DeleteBehavior.Cascade);

                     builder.HasMany<SecondaryBarcode>()
                            .WithOne()
                            .HasForeignKey(b => b.DocumentId)
                            .OnDelete(DeleteBehavior.Cascade);

                     builder.HasMany<TertiaryBarcode>()
                            .WithOne()
                            .HasForeignKey(b => b.DocumentId)
                            .OnDelete(DeleteBehavior.Cascade);
              }
       }
}
