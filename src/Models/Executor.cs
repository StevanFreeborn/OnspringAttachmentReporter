using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Onspring.API.SDK;
using OnspringAttachmentReporter.Interfaces;
using OnspringAttachmentReporter.Services;
using Serilog;
using Serilog.Events;

namespace OnspringAttachmentReporter.Models;

public static class Executor
{
  public static async Task<int> Execute(string? apiKeyOption, int? appIdOption, string? configFileOption, LogEventLevel logLevelOption)
  {
    var outputDirectory = $"{DateTime.Now:yyyyMMddHHmm}-output";

    var logPath = LoggerFactory.GetLogPath(outputDirectory);
    Log.Logger = LoggerFactory.CreateLogger(logPath, logLevelOption);

    Log.Information("Starting Onspring Attachment Reporter.");
    Log.Debug("Attempting to get context from config file or command line options.");

    var appContext = GetContext(apiKeyOption, appIdOption, configFileOption, outputDirectory);

    if (appContext is null)
    {
      Log.Fatal("Unable to get context from config file or command line options.");
      return 1;
    }

    Log.Debug("Context successfully retrieved from config file or command line options.");

    var onspringClient = new OnspringClient("https://api.onspring.com", appContext.ApiKey);
    var host = Host.CreateDefaultBuilder()
      .ConfigureServices((context, services) =>
      {
        services.AddSingleton<IContext>(appContext);
        services.AddSingleton(Log.Logger);
        services.AddSingleton<IOnspringClient>(onspringClient);
        services.AddSingleton<IOnspringService, OnspringService>();
        services.AddSingleton<IReportService, ReportService>();
        services.AddSingleton<IProcessor, Processor>();
        services.AddSingleton<Runner>();
      })
      .Build();

    var runner = ActivatorUtilities.CreateInstance<Runner>(host.Services);
    var result = await runner.Run();

    Log.Information("Onspring Attachment Reporter finished.");
    Log.Information("You can find the log and report files in the output directory: {OutputDirectory}", outputDirectory);

    return result;
  }

  [ExcludeFromCodeCoverage]
  private static Context? GetContext(string? apiKeyOption, int? appIdOption, string? configFileOption, string outputDirectory)
  {
    if (
      string.IsNullOrWhiteSpace(apiKeyOption) is false &&
      appIdOption is not null
    )
    {
      return new Context(apiKeyOption!, appIdOption.Value, outputDirectory);
    }

    if (string.IsNullOrWhiteSpace(configFileOption) is false)
    {
      var config = new ConfigurationBuilder()
        .AddJsonFile(configFileOption, optional: true, reloadOnChange: true)
        .Build();

      var apiKey = config["ApiKey"];
      var appId = config["AppId"];

      if (
        apiKey is not null &&
        appId is not null &&
        int.TryParse(appId, out var appIdInt) is true
      )
      {
        return new Context(apiKey, appIdInt, outputDirectory);
      }
    }

    return null;
  }
}