using OnspringAttachmentReporter.Extensions;

var apiKeyOption = new Option<string>(
  aliases: new string[] { "--apikey", "-k" },
  description: "The API key that will be used to authenticate with Onspring."
);

var appIdOption = new Option<int?>(
  aliases: new string[] { "--appId", "-a" },
  description: "The ID of the Onspring app that will be reported on.",
  getDefaultValue: () => null
);

var configFileOption = new Option<string>(
  aliases: new string[] { "--config", "-c" },
  description: "The path to the file that specifies configuration for the reporter."
);

var logLevelOption = new Option<LogEventLevel>(
  aliases: new string[] { "--log", "-l" },
  description: "Set the minimum level of event that will be logged to the console.",
  getDefaultValue: () => LogEventLevel.Information
);

var rootCommand = new RootCommand("An app that will report on all attachments in a given Onspring app.")
{
  apiKeyOption,
  appIdOption,
  configFileOption,
  logLevelOption
};

rootCommand.SetHandler(
  async (apiKeyOption, appIdOption, configFileOption, logLevelOption) =>
    await Reporter
      .GetContext(apiKeyOption, appIdOption, logLevelOption, configFileOption)
      .ConfigureServices()
      .BuildServiceProvider()
      .GetRequiredService<Reporter>()
      .Run(),
    apiKeyOption,
    appIdOption,
    configFileOption,
    logLevelOption
);

return await rootCommand.InvokeAsync(args);