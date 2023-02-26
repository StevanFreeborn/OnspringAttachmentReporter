namespace OnspringAttachmentReporter.Models;

public class FileInfo
{
  public int RecordId { get; set; }
  public int FieldId { get; set; }
  public int FileId { get; set; }
  public string? FileName { get; set; }
  public decimal FileSizeInBytes { get; set; }
  public decimal FileSizeInGB => Math.Round(FileSizeInBytes / 1000, 4);
  public decimal FileSizeInGiB => Math.Round(FileSizeInBytes / 1024, 4);

  public FileInfo(int recordId, int fieldId, int fileId, string? fileName, decimal fileSizeInBytes)
  {
    RecordId = recordId;
    FieldId = fieldId;
    FileId = fileId;
    FileName = fileName;
    FileSizeInBytes = fileSizeInBytes;
  }
}