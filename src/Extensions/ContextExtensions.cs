namespace OnspringAttachmentReporter.Extensions;

public static class ContextExtensions
{
  public static IServiceCollection ConfigureServices(this Context context)
  {
    var logger = LoggerFactory.CreateLogger(context.LogLevel, context.OutputDirectory);
    var onspringClient = new OnspringClient("https://api.onspring.com", context.ApiKey);

    return new ServiceCollection()
    .AddSingleton<IContext>(context)
    .AddSingleton<ILogger>(logger)
    .AddSingleton<IOnspringClient>(onspringClient)
    .AddSingleton<IOnspringService, OnspringService>()
    .AddSingleton<IReportService, ReportService>()
    .AddSingleton<IProcessor, Processor>()
    .AddSingleton<Reporter>();
  }
}