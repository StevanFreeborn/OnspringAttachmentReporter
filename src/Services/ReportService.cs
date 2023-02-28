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
      csv.WriteRecords(fileInfos);
    };
  }

  internal string GetReportPath()
  {
    var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
    var outputDirectory = Path.Combine(currentDirectory, _context.OutputDirectory);

    if (!Directory.Exists(outputDirectory))
    {
      Directory.CreateDirectory(outputDirectory);
    }

    return Path.Combine(currentDirectory, _context.OutputDirectory, "attachment_report.csv");
  }
}