using FileInfo = OnspringAttachmentReporter.Models.FileInfo;

namespace OnspringAttachmentReporter.Interfaces;

public interface IReportService
{
  void WriteReport(List<FileInfo> fileInfos);
}