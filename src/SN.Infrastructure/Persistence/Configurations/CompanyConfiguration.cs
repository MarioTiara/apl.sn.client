

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SN.Core.Domain.Companies;

namespace SN.Infrastructure.Persistence.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
       public void Configure(EntityTypeBuilder<Company> builder)
       {
              // Table name
              builder.ToTable("Companies");

              // Primary key
              builder.HasKey(c => c.Id);

              // Properties
              builder.Property(c => c.CompanyCode)
                     .IsRequired()
                     .HasMaxLength(50);

              builder.Property(c => c.CompanyName)
                     .IsRequired()
                     .HasMaxLength(255);

              builder.Property(c => c.BpomSaranaId);

              builder.Property(c => c.BpomUsername)
                     .HasMaxLength(100);

              builder.Property(c => c.BpomPassword)
                     .HasMaxLength(100);

              builder.Property(c => c.BpomToken)
                     .HasMaxLength(255);

              builder.Property(c => c.LastUpdatedToken);
              // builder.
              builder.Property(c => c.CreatedAt);

              // Index (optional) for CompanyCode to speed up lookup
              builder.HasIndex(c => c.CompanyCode)
               .IsUnique();
       }
}

