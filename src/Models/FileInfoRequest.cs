namespace OnspringAttachmentReporter.Models;

public class FileInfoRequest
{
  public int RecordId { get; set; }
  public int FieldId { get; set; }
  public int FileId { get; set; }

  public FileInfoRequest(int recordId, int fieldId, int fileId)
  {
    RecordId = recordId;
    FieldId = fieldId;
    FileId = fileId;
  }
}