namespace OnspringAttachmentReporter.Models;

class Processor : IProcessor
{
  private readonly IOnspringService _onspringService;
  private readonly IReportService _reportService;
  private readonly ILogger _logger;

  public Processor(IOnspringService onspringService, IReportService reportService, ILogger logger)
  {
    _onspringService = onspringService;
    _reportService = reportService;
    _logger = logger;
  }

  public async Task<List<Field>> GetFileFields()
  {
    var fields = await _onspringService.GetAllFields();

    return fields
    .Where(f => f.Type == FieldType.Attachment || f.Type == FieldType.Image)
    .ToList();
  }

  public async Task<List<FileInfoRequest>> GetFileRequests(List<Field> fileFields)
  {
    var fileFieldIds = fileFields.Select(f => f.Id).ToList();
    var pagingRequest = new PagingRequest(1, 50);
    var totalPages = 1;
    var currentPage = pagingRequest.PageNumber;
    var fileRequests = new List<FileInfoRequest>();

    do
    {
      _logger.Debug(
        "Retrieving records for page {PageNumber}.",
        currentPage
      );

      var res = await _onspringService.GetAPageOfRecords(fileFieldIds, pagingRequest);

      pagingRequest.PageNumber++;
      currentPage = pagingRequest.PageNumber;

      if (res == null)
      {
        _logger.Warning(
          "No records found for page {PageNumber}.",
          currentPage
        );
        continue;
      }

      _logger.Debug(
        "Records retrieved for page {PageNumber}. {Count} records found.",
        currentPage,
        res.Items.Count
      );

      foreach (var record in res.Items)
      {
        var requests = GetFileRequestsFromRecord(record, fileFields);

        foreach (var request in requests)
        {
          fileRequests.Add(request);
        }
      }

      totalPages = res.TotalPages;
    } while (currentPage <= totalPages);

    return fileRequests;
  }

  public async Task<List<FileInfo>> GetFileInfos(List<FileInfoRequest> fileRequests)
  {
    var fileInfos = new List<FileInfo>();

    await Parallel.ForEachAsync(fileRequests, async (fileRequest, token) =>
    {
      var fileInfo = await GetFileInfo(fileRequest);
      fileInfos.Add(fileInfo);
    });

    return fileInfos;
  }

  public void PrintReport(List<FileInfo> fileInfos)
  {
    _reportService.WriteReport(fileInfos);
  }

  internal static List<FileInfoRequest> GetFileRequestsFromRecord(ResultRecord record, List<Field> fileFields)
  {
    var fileRequests = new List<FileInfoRequest>();

    foreach (var fieldValue in record.FieldData)
    {
      var field = fileFields.FirstOrDefault(f => f.Id == fieldValue.FieldId);

      if (field == null)
      {
        continue;
      }

      if (field.Type == FieldType.Attachment)
      {
        var attachments = fieldValue.AsAttachmentList();

        if (IsAllAttachmentsField(record, fileFields, attachments))
        {
          continue;
        }

        foreach (var attachment in attachments)
        {
          if (attachment.StorageLocation != FileStorageSite.Internal)
          {
            continue;
          }

          fileRequests.Add(
            new FileInfoRequest(
              record.RecordId,
              fieldValue.FieldId,
              field.Name,
              attachment.FileId
            )
          );
        }
      }

      if (field.Type == FieldType.Image)
      {
        var files = fieldValue.AsFileList();

        foreach (var file in files)
        {
          fileRequests.Add(
            new FileInfoRequest(
              record.RecordId,
              fieldValue.FieldId,
              field.Name,
              file
            )
          );
        }
      }
    }

    return fileRequests;
  }

  internal static bool IsAllAttachmentsField(ResultRecord record, List<Field> fileFields, List<AttachmentFile> attachmentFieldValue)
  {
    var attachmentFieldIds = fileFields
    .Where(f => f.Type == FieldType.Attachment)
    .Select(f => f.Id)
    .ToList();

    // If there is only one attachment field, then there can't be "All Attachments" field.
    if (attachmentFieldIds.Count <= 1)
    {
      return false;
    }

    var attachmentIds = record.FieldData
    .Where(f => attachmentFieldIds.Contains(f.FieldId))
    .SelectMany(f => f.AsAttachmentList())
    .Select(f => f.FileId)
    .Distinct()
    .ToList();

    return attachmentFieldValue.Select(f => f.FileId).Distinct().SequenceEqual(attachmentIds);
  }

  internal async Task<FileInfo> GetFileInfo(FileInfoRequest fileRequest)
  {
    _logger.Debug(
      "Retrieving file info for record {RecordId}, field {FieldId}, file {FileId}.",
      fileRequest.RecordId,
      fileRequest.FieldId,
      fileRequest.FileId
    );

    var res = await _onspringService.GetFile(fileRequest);

    if (res == null)
    {
      _logger.Warning(
        "Unable to get file info for record {RecordId}, field {FieldId}, file {FileId}.",
        fileRequest.RecordId,
        fileRequest.FieldId,
        fileRequest.FileId
      );

      return new FileInfo(
        fileRequest.RecordId,
        fileRequest.FieldId,
        fileRequest.FieldName,
        fileRequest.FileId,
        "Error: Unable to get file info",
        0
      );
    }

    _logger.Debug(
      "File info retrieved for record {RecordId}, field {FieldId}, file {FileId}.",
      fileRequest.RecordId,
      fileRequest.FieldId,
      fileRequest.FileId
    );

    return new FileInfo(
      fileRequest.RecordId,
      fileRequest.FieldId,
      fileRequest.FieldName,
      fileRequest.FileId,
      res.FileName,
      Convert.ToDecimal(res.ContentLength)
    );
  }
}