// See https://aka.ms/new-console-template for more information
// using SN.AZIParser;


// var path = @"Docs\Sample EPCIS File from AZ to APL.xml";
// var parser = new AZIEPCISParser();
// var snDocument = parser.ParseToSNDocument(path);

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SN.Applications.Documents;
using SN.AZIParser;
using SN.Infrastructure.EPCIS;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("== EPCPRODUCTION Worker ==");

        var hostBuilder = CreateHostBuilder(args);

        await hostBuilder.RunConsoleAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddEnvironmentVariables()
                      .AddCommandLine(args);
            })
            // .UseSerilog((context, config) =>
            // {
            //     config.ReadFrom.Configuration(context.Configuration);
            // })
       .ConfigureServices((hostingContext, services) =>
       {
           var configuration = hostingContext.Configuration;
           services.UseInfrastructure(configuration);
           services.AddScoped<IAggregationBuilder, EPCISAgregationBuilder>();
           services.AddSingleton<IHostedService, ConsoleApp>();
       });
}
