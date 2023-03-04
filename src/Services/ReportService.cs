using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

using FileInfo = OnspringAttachmentReporter.Models.FileInfo;

namespace OnspringAttachmentReporter.Services;

public class ReportService : IReportService
{
  private readonly IContext _context;

  public ReportService(IContext context)
  {
    _context = context;
  }

  public void WriteReport(List<FileInfo> fileInfos)
  {
    var fileName = GetReportPath();
    using var writer = new StreamWriter(fileName);

    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
      ShouldQuote = (field) => false,
    };

    using (var csv = new CsvWriter(writer, config))
    {
      csv.Context.RegisterClassMap<FileInfoMap>();

      var options = new ProgressBarOptions
      {
        ForegroundColor = ConsoleColor.DarkBlue,
        ProgressCharacter = '─',
        ShowEstimatedDuration = false,
      };

      using var progressBar = new ProgressBar(
        fileInfos.Count,
        "Writing report...",
        options
      );

      csv.WriteHeader<FileInfo>();
      csv.NextRecord();

      foreach (var fileInfo in fileInfos)
      {
        csv.WriteRecord(fileInfo);
        csv.NextRecord();
        progressBar.Tick($"Wrote file {fileInfo.FileId} to report.");
      }

      progressBar.Message = "Finished writing report.";
    };
  }

  internal string GetReportPath()
  {
    var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
    var outputDirectory = Path.Combine(
      currentDirectory,
      _context.OutputDirectory
    );

    if (!Directory.Exists(outputDirectory))
    {
      Directory.CreateDirectory(outputDirectory);
    }

    return Path.Combine(
      currentDirectory,
      _context.OutputDirectory,
      "attachment_report.csv"
    );
  }
}