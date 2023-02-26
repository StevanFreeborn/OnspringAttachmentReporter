namespace OnspringAttachmentReporter.Interfaces
{

  public interface IOnspringService
  {
    Task<List<Field>> GetAllFields(int pageSize = 50);
    Task<GetPagedRecordsResponse?> GetAPageOfRecords(List<int> fileFields, PagingRequest pagingRequest);
    Task<GetFileResponse?> GetFile(FileInfoRequest fileRequest);
  }
}