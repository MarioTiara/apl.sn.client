using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SN.Core.Domain.Companies;
using SN.Core.Domain.SAPIntegration;
using SN.Infrastructure.Persistence;

namespace SN.AZIParser;

public class MainService
{
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<MainService> _logger;

    public MainService(IHostApplicationLifetime applicationLifetime, IServiceProvider serviceProvider, ILogger<MainService> logger)
    {
        _applicationLifetime = applicationLifetime;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public void Run()
    {
        try
        {
            string folderPath = @"Docs";
            string[] files = Directory.GetFiles(folderPath, "*.xml");

            var context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            var parser = new AZIEPCISParser();
            var companyOwner = context.Companies.FirstOrDefault(c => c.CompanyCode == "AZI");

            foreach (var filePath in files)
            {
                try
                {
                    ProcessFile(filePath, context, parser, companyOwner ?? new Company("AZI", "Azi company"));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error processing file {filePath}");
                    continue;
                }

            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Fatal error in Run()");
        }
    }
    private void ProcessFile(string filePath, ApplicationDbContext context, AZIEPCISParser parser, Company companyOwner)
    {
        try
        {
            var snDocument = parser.ParseToSNDocument(filePath, companyOwner ?? throw new Exception("Company not found"));
            var SAPDataSyncLogs = snDocument.PrimaryBarcodes
                .Select(pb => new SAPDataSyncLog(pb.Detail))
                .ToList();

            using var transaction = context.Database.BeginTransaction();
            try
            {
                context.Add(snDocument);
                context.AddRange(SAPDataSyncLogs);
                context.SaveChanges();

                transaction.Commit();
                _logger.LogInformation($"Successfully processed file {filePath}");

                MoveFileToProcessedFolder(filePath);
            }
            catch (Exception dbEx)
            {
                transaction.Rollback();
                _logger.LogError(dbEx, $"Database error for file {filePath}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Parsing error for file {filePath}");
        }
    }

    private void MoveFileToProcessedFolder(string filePath)
    {
        var processedFolder = Path.Combine(Path.GetDirectoryName(filePath) ?? string.Empty, "Processed");
        if (!Directory.Exists(processedFolder))
        {
            Directory.CreateDirectory(processedFolder);
        }
        var destFilePath = Path.Combine(processedFolder, Path.GetFileName(filePath));
        if (File.Exists(destFilePath))
        {
            File.Delete(destFilePath);
        }
        File.Move(filePath, destFilePath);
    }
}