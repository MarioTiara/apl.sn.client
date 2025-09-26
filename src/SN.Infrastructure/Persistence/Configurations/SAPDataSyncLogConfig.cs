namespace SN.Infrastructure.Persistence.Configurations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SN.Core.Domain.SAPIntegration;

public class SAPDataSyncLogConfig : IEntityTypeConfiguration<SAPDataSyncLog>
{
    public void Configure(EntityTypeBuilder<SAPDataSyncLog> builder)
    {
        builder.ToTable("SAPDataSyncLogs");

        builder.HasKey(x => x.Id); // Asumsikan BaseEntity punya Id

        builder.Property(x => x.BarcodeId)
            .IsRequired();

        builder.HasOne(x => x.Barcode)
            .WithMany() // Jika Barcode tidak punya koleksi log
            .HasForeignKey(x => x.BarcodeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(x => x.IsSuccess)
            .IsRequired();

        builder.Property(x => x.ResponseMessage)
            .HasMaxLength(1000); // Batas panjang respons

        builder.Property(x => x.RetryCount)
            .IsRequired();

        builder.Property(x => x.LastRetryAt);

        // Index untuk performa query
        builder.HasIndex(x => x.BarcodeId);
        builder.HasIndex(x => x.IsSuccess);
    }
}