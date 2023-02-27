using CsvHelper.Configuration;

namespace OnspringAttachmentReporter.Models;

[ExcludeFromCodeCoverage]
public class FileInfoMap : ClassMap<FileInfo>
{
  public FileInfoMap()
  {
    Map(m => m.RecordId).Name("Record Id");
    Map(m => m.FieldId).Name("Field ID");
    Map(m => m.FieldName).Name("Field Name");
    Map(m => m.FileId).Name("File Id");
    Map(m => m.FileName).Name("File Name");
    Map(m => m.FileSizeInBytes).Name("File Size (Bytes)");
    Map(m => m.FileSizeInKB).Name("File Size (KB)");
    Map(m => m.FileSizeInKiB).Name("File Size (KiB)");
    Map(m => m.FileSizeInMB).Name("File Size (MB)");
    Map(m => m.FileSizeInMiB).Name("File Size (MiB)");
    Map(m => m.FileSizeInGB).Name("File Size (GB)");
    Map(m => m.FileSizeInGiB).Name("File Size (GiB)");
  }
}