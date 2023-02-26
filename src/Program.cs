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

var rootCommand = new RootCommand("An app that will report on all attachments in a given Onspring app.");
rootCommand.AddOption(apiKeyOption);
rootCommand.AddOption(appIdOption);
rootCommand.AddOption(configFileOption);
rootCommand.AddOption(logLevelOption);
rootCommand.SetHandler(
  Executor.Execute,
  apiKeyOption,
  appIdOption,
  configFileOption,
  logLevelOption
);

return await rootCommand.InvokeAsync(args);