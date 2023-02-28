namespace OnspringAttachmentReporterTests.UnitTests;

public class FileInfoRequestTests
{
  [Fact]
  public void FileInfoRequest_WhenConstructorIsCalled_ItShouldReturnANewINstance()
  {
    var recordId = 1;
    var fieldId = 2;
    var fieldName = "Test Field";
    var fileId = 3;
    var fileInfoRequest = new FileInfoRequest(recordId, fieldId, fieldName, fileId);

    fileInfoRequest.RecordId.Should().Be(recordId);
    fileInfoRequest.FieldId.Should().Be(fieldId);
    fileInfoRequest.FieldName.Should().Be(fieldName);
    fileInfoRequest.FileId.Should().Be(fileId);
  }
}