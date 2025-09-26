using Microsoft.EntityFrameworkCore;
using SN.Core.Domain.Companies;
using SN.Core.Domain.Documents;
using SN.Core.Domain.Barcodes;
using SN.Core.Domain.SerialNode;
using System.Reflection;

namespace SN.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets
        public DbSet<Company> Companies { get; set; }
        public DbSet<SNDocument> Documents { get; set; }
        public DbSet<SerializedNode> SerializedNodes { get; set; }

        // Barcode hierarchy
        public DbSet<Barcode> Barcodes { get; set; }
        public DbSet<TertiaryBarcode> TertiaryBarcodes { get; set; }
        public DbSet<SecondaryBarcode> SecondaryBarcodes { get; set; }
        public DbSet<PrimaryBarcode> PrimaryBarcodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            // Auto-apply all IEntityTypeConfiguration<T> in the assembly
            // modelBuilder.Ignore<IBarcodeAgregation>(); 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
