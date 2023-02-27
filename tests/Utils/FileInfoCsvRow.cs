namespace OnspringAttachmentReporterTests.Utils;

public class FileInfoCsvRow
{
  public int RecordId { get; set; }
  public int FieldId { get; set; }
  public string? FieldName { get; set; }
  public int FileId { get; set; }
  public string? FileName { get; set; }
  public decimal FileSizeInBytes { get; set; }
  public decimal FileSizeInKB { get; set; }
  public decimal FileSizeInKiB { get; set; }
  public decimal FileSizeInMB { get; set; }
  public decimal FileSizeInMiB { get; set; }
  public decimal FileSizeInGB { get; set; }
  public decimal FileSizeInGiB { get; set; }
}