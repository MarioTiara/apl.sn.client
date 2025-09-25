

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SN.Core.Domain.Companies;
using SN.Infrastructure.Persistence;

namespace SN.AZIParser;

public class ConsoleApp : IHostedService
{
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ConsoleApp> _logger;
    public ConsoleApp(IHostApplicationLifetime applicationLifetime, IServiceProvider serviceProvider, ILogger<ConsoleApp> loggger)
    {
        _applicationLifetime = applicationLifetime;
        _serviceProvider = serviceProvider;
        _logger = loggger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {

        var path = @"Docs\Receiver_180Days_9a586fdd-4c25-42bd-bf21-d68436988765_2025-09-10T08_05_33.809Z.xml";
        var context = _serviceProvider.GetRequiredService<ApplicationDbContext>();
        var parser = new AZIEPCISParser();
        var companyOwner = context.Companies.FirstOrDefault(c => c.CompanyCode == "AZI");
        var snDocument = parser.ParseToSNDocument(path, companyOwner?? new Company("AZI", "Azi company"));
        using var transaction = await context.Database.BeginTransactionAsync();
        try
        {


            // context.Documents.
            // Save all changes in one transaction
            context.Add(snDocument);
            // context.Add(snDocument.Barcodes);
            await context.SaveChangesAsync();

            // Commit transaction
            await transaction.CommitAsync();
        }
        catch (Exception ex)
        {
            // Rollback if anything failed
            await transaction.RollbackAsync();
            throw; // or log
        }


        await this.StopAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _applicationLifetime.StopApplication();
        return Task.CompletedTask;
    }


}
