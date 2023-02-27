using FileInfo = OnspringAttachmentReporter.Models.FileInfo;

namespace OnspringAttachmentReporterTests.UnitTests;

public class FileInfoTests
{
  [Fact]
  public void FileInfo_WhenConstructorIsCalled_ItShouldReturnANewINstance()
  {
    var recordId = 1;
    var fieldId = 2;
    var fieldName = "Test Field";
    var fileId = 3;
    var fileName = "Test File";
    var fileSizeInBytes = 1000000000;
    var fileInfo = new FileInfo(recordId, fieldId, fieldName, fileId, fileName, fileSizeInBytes);

    fileInfo.RecordId.Should().Be(recordId);
    fileInfo.FieldId.Should().Be(fieldId);
    fileInfo.FieldName.Should().Be(fieldName);
    fileInfo.FileId.Should().Be(fileId);
    fileInfo.FileName.Should().Be(fileName);
    fileInfo.FileSizeInBytes.Should().Be(fileSizeInBytes);
    fileInfo.FileSizeInBytes.Should().Be(1000000000);
    fileInfo.FileSizeInKB.Should().Be(1000000);
    fileInfo.FileSizeInKiB.Should().Be(Convert.ToDecimal(976562.5));
    fileInfo.FileSizeInMB.Should().Be(1000);
    fileInfo.FileSizeInMiB.Should().Be(Convert.ToDecimal(953.6743));
    fileInfo.FileSizeInGB.Should().Be(1);
    fileInfo.FileSizeInGiB.Should().Be(Convert.ToDecimal(0.9313));
  }
}