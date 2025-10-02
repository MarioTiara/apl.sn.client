

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace SN.AZIParser;

public class ConsoleApp : IHostedService
{
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly MainService mainService;

    public ConsoleApp(IHostApplicationLifetime applicationLifetime, IServiceProvider serviceProvider)
    {
        _applicationLifetime = applicationLifetime;
        mainService = serviceProvider.GetRequiredService<MainService>();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            mainService.Run();
        }
        catch (Exception ex)
        {
            // Log fatal error and optionally decide whether to stop
            Console.WriteLine($"Fatal error: {ex.Message}");
        }
        finally
        {
            await this.StopAsync(cancellationToken);
        }

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _applicationLifetime.StopApplication();
        return Task.CompletedTask;
    }

}
