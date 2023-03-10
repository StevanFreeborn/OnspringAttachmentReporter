namespace OnspringAttachmentReporter.Models;

public class FileInfoRequest
{
  public int RecordId { get; set; }
  public int FieldId { get; set; }
  public string FieldName { get; set; }
  public int FileId { get; set; }

  public FileInfoRequest(
    int recordId,
    int fieldId,
    string fieldName,
    int fileId
  )
  {
    RecordId = recordId;
    FieldId = fieldId;
    FieldName = fieldName;
    FileId = fileId;
  }
}