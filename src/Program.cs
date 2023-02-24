using System.CommandLine;
using Serilog.Events;

class Program
{
  static async Task<int> Main(string[] args)
  {
    var apiKeyOption = new Option<string>(
      aliases: new string[] { "--apikey", "-k" },
      description: "The API key that will be used to authenticate with Onspring.",
      getDefaultValue: () => null
    );

    var appIdOption = new Option<string>(
      aliases: new string[] { "--appid", "-a" },
      description: "The ID of the Onspring app that will be reported on.",
      getDefaultValue: () => null
    );

    var configFileOption = new Option<string>(
      aliases: new string[] { "--config", "-c" },
      description: "The path to the file that specifies configuration for the reporter.",
      getDefaultValue: () => null
    );

    var logLevelOption = new Option<LogEventLevel>(
      aliases: new string[] { "--log", "-l" },
      description: "Set the minimum level of event that will be logged to the console.",
      getDefaultValue: () => LogEventLevel.Information
    );

    var rootCommand = new RootCommand("An app that will report on all attachments in a given Onspring app.");
    rootCommand.AddOption(apiKeyOption);
    rootCommand.AddOption(appIdOption);
    rootCommand.AddOption(configFileOption);
    rootCommand.AddOption(logLevelOption);
    rootCommand.SetHandler(Run, apiKeyOption, appIdOption, configFileOption, logLevelOption);

    return await rootCommand.InvokeAsync(args);
  }

  static async Task<int> Run(string apiKeyOption, string appIdOption, string configFileOption, LogEventLevel logLevelOption)
  {
    // setup logger
    // check for config
    // needs to come from command line options or config file
    // command line options will supersede config file if both are specified
    // if not found, log error and exit

    // get all attachment or image fields
    // get a page of records
    // for each record get the file info for each file in each attachment or image field
    // for each file write it's file info to a csv file
    return 0;
  }
}