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
                     builder.Property(d => d.FilePath).HasMaxLength(255);
                     builder.Property(d => d.DocumentCreationTime);
                     builder.Property(d => d.DocumentType).HasMaxLength(25);
                     builder.Property(d => d.DocumentIdentifier).HasMaxLength(255);
                     builder.Property(d => d.DeliveryNumber).HasMaxLength(50);
                     builder.Property(d => d.SenderIdentifier).IsRequired().HasMaxLength(50);
                     builder.Property(d => d.ReceiverIdentifier).IsRequired().HasMaxLength(50);
                     builder.Property(d => d.CreatedAt).IsRequired();

                     // Relationship: SNDocument -> Producer (Company)
                     builder.HasOne(d => d.Producer)
                            .WithMany()
                            .IsRequired()
                            .OnDelete(DeleteBehavior.Restrict);

                     // Relationship: SNDocument -> SerializedNodes
                     builder.HasMany(d => d.EpcisNodes)
                            .WithOne()
                            .HasForeignKey(e => e.DocumentId)
                            .OnDelete(DeleteBehavior.Cascade);

                     // Ignore the private Barcodes collection for EF mapping
                     // Barcodes (private backing field)
                     builder.HasMany(typeof(PrimaryBarcode), "_primaries")
                            .WithOne("Document")
                            .HasForeignKey("DocumentId")
                            .OnDelete(DeleteBehavior.Cascade);

                     builder.HasMany(typeof(SecondaryBarcode), "_secondaries")
                            .WithOne("Document")
                            .HasForeignKey("DocumentId")
                            .OnDelete(DeleteBehavior.Cascade);

                     builder.HasMany(typeof(TertiaryBarcode), "_tertiaries")
                            .WithOne("Document")
                            .HasForeignKey("DocumentId")
                            .OnDelete(DeleteBehavior.Cascade);
              }
       }
}
