using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;

namespace OnspringAttachmentReporter.Models;

class Runner
{
  public static async Task<int> Run(string? apiKeyOption, int? appIdOption, string? configFileOption, LogEventLevel logLevelOption)
  {
    var logPath = LoggerFactory.GetLogPath();
    Log.Logger = LoggerFactory.CreateLogger(logPath, logLevelOption);

    var context = GetContext(apiKeyOption, appIdOption, configFileOption);

    if (context is null)
    {
      Log.Fatal("Unable to get context from config file or command line options.");
      return 1;
    }

    var processor = new Processor(context);

    // get all attachment or image fields
    // get a page of records
    // for each record get the file info for each file in each attachment or image field
    // for each file write it's file info to a csv file
    return 0;
  }

  static Context? GetContext(string? apiKeyOption, int? appIdOption, string? configFileOption)
  {
    if (
      string.IsNullOrWhiteSpace(apiKeyOption) is false &&
      appIdOption is not null
    )
    {
      return new Context(apiKeyOption!, appIdOption.Value);
    }

    if (string.IsNullOrWhiteSpace(configFileOption) is false)
    {
      var config = new ConfigurationBuilder()
        .AddJsonFile(configFileOption!)
        .Build();

      var apiKey = config["ApiKey"];
      var appId = config["AppId"];

      if (
        apiKey is not null &&
        appId is not null &&
        int.TryParse(appId, out var appIdInt)
      )
      {
        return new Context(apiKey, appIdInt);
      }
    }

    Log.Error("No valid api key and/or app id was provided.");
    return null;
  }
}