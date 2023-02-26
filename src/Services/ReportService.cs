using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using CsvHelper;
using OnspringAttachmentReporter.Interfaces;
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
    using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
    csv.WriteRecords(fileInfos);
  }

  [ExcludeFromCodeCoverage]
  private string GetReportPath()
  {
    var currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
    return Path.Combine(currentDirectory, _context.OutputDirectory, "attachment_report.csv");
  }
}