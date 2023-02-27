using System.Globalization;

using CsvHelper;
using CsvHelper.Configuration;

using FileInfo = OnspringAttachmentReporter.Models.FileInfo;

namespace OnspringAttachmentReporterTests.UnitTests;

public class ReportServiceTests
{
  private readonly IContext _context;

  public ReportServiceTests()
  {
    _context = new Context("apikey", 1, "testOutput", LogEventLevel.Information);
  }

  [Fact]
  public void WriteReport_WhenCalled_ItShouldWriteToCorrectFile()
  {
    var fileInfos = new List<FileInfo>()
    {
      new FileInfo(1, 1, "test", 1, "test", 1000000000),
    };

    var reportService = new ReportService(_context);

    reportService.WriteReport(fileInfos);
    var result = File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _context.OutputDirectory, "attachment_report.csv"));
    result.Should().BeTrue();
  }

  [Fact]
  public void WriteReport_WhenCalled_ItShouldWriteCorrectData()
  {
    var fileInfos = new List<FileInfo>()
    {
      new FileInfo(1, 1, "test", 1, "test", 1000000000),
    };

    var reportService = new ReportService(_context);

    reportService.WriteReport(fileInfos);

    var path = Path.Combine(_context.OutputDirectory, "attachment_report.csv");
    using var reader = new StreamReader(path);

    using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
    csv.Context.RegisterClassMap<FileInfoCsvRowMap>();
    var records = csv.GetRecords<FileInfoCsvRow>();
    foreach (var record in records)
    {
      record.RecordId.Should().Be(1);
      record.FieldId.Should().Be(1);
      record.FieldName.Should().Be("test");
      record.FileId.Should().Be(1);
      record.FileName.Should().Be("test");
      record.FileSizeInBytes.Should().Be(1000000000);
      record.FileSizeInKB.Should().Be(1000000);
      record.FileSizeInKiB.Should().Be(Convert.ToDecimal(976562.5));
      record.FileSizeInMB.Should().Be(1000);
      record.FileSizeInMiB.Should().Be(Convert.ToDecimal(953.6743));
      record.FileSizeInGB.Should().Be(1);
      record.FileSizeInGiB.Should().Be(Convert.ToDecimal(0.9313));
    }
  }
}