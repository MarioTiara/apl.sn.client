using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SN.Core.Domain.Documents;
using SN.Core.Domain.SerialNode;

namespace SN.Infrastructure.Persistence.Configurations
{
    public class SerializedNodeConfiguration : IEntityTypeConfiguration<SerializedNode>
    {
        public void Configure(EntityTypeBuilder<SerializedNode> builder)
        {
            // Table name
            builder.ToTable("SerializedNodes");
            // Primary key
            builder.HasKey(e => e.Id);
            // Properties
            builder.Property(e => e.SerializedCode)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(e => e.Level)
                   .IsRequired()
                   .HasConversion<string>(); // Enum to string

            builder.Property(e => e.DocumentId)
                   .IsRequired();

            builder.Property(e => e.ParentId);
            builder.Property(e => e.CreatedAt);

            // Relationships
            builder.HasOne<SNDocument>()  // SerializedNode belongs to a SNDocument
                   .WithMany()            // No navigation property on SNDocument (optional)
                   .HasForeignKey(e => e.DocumentId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Self-referencing parent-child relationship
            builder.HasOne<SerializedNode>()
                   .WithMany()
                   .HasForeignKey(e => e.ParentId)
                   .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete for parent
        }
    }
}
