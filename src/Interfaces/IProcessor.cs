using FileInfo = OnspringAttachmentReporter.Models.FileInfo;

namespace OnspringAttachmentReporter.Interfaces;

public interface IProcessor
{
  Task<List<Field>> GetFileFields();
  Task<List<FileInfoRequest>> GetFileRequests(List<Field> fileFields);
  Task<List<FileInfo>> GetFileInfos(List<FileInfoRequest> fileRequests);
  void PrintReport(List<FileInfo> fileInfos);
}