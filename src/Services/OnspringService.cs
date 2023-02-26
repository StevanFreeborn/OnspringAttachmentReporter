using System.Diagnostics.CodeAnalysis;
using Onspring.API.SDK;
using Onspring.API.SDK.Models;
using OnspringAttachmentReporter.Interfaces;
using OnspringAttachmentReporter.Models;
using Serilog;

namespace OnspringAttachmentReporter.Services;

class OnspringService : IOnspringService
{
  private readonly IContext _context;
  private readonly IOnspringClient _client;
  private readonly ILogger _logger;

  public OnspringService(IContext context, IOnspringClient client, ILogger logger)
  {
    _context = context;
    _client = client;
    _logger = logger;
  }

  public async Task<List<Field>> GetAllFields(int pageSize = 50)
  {
    var fields = new List<Field>();
    var totalPages = 1;
    var pagingRequest = new PagingRequest(1, pageSize);
    var currentPage = pagingRequest.PageNumber;

    do
    {
      var res = await ExecuteRequest(async () => await _client.GetFieldsForAppAsync(_context.AppId, pagingRequest));

      if (res.IsSuccessful is true)
      {
        fields.AddRange(res.Value.Items);
        totalPages = res.Value.TotalPages;
      }
      else
      {
        _logger.Error(
          "Unable to get fields. {StatusCode} - {Message}. Current page: {CurrentPage}. Total pages: {TotalPages}.",
          res.StatusCode,
          res.Message,
          currentPage,
          totalPages
        );
      }

      currentPage += pagingRequest.PageNumber;
    } while (currentPage <= totalPages);

    return fields;
  }

  public async Task<GetPagedRecordsResponse?> GetAPageOfRecords(List<int> fileFields, PagingRequest pagingRequest)
  {
    var request = new GetRecordsByAppRequest
    {
      AppId = _context.AppId,
      PagingRequest = pagingRequest,
      FieldIds = fileFields
    };

    var res = await ExecuteRequest(async () => await _client.GetRecordsForAppAsync(request));

    if (res.IsSuccessful is true)
    {
      return res.Value;
    }

    _logger.Error(
      "Unable to get records. {StatusCode} - {Message}.",
      res.StatusCode,
      res.Message
    );

    return null;
  }

  public async Task<GetFileResponse?> GetFile(FileInfoRequest fileRequest)
  {
    var res = await ExecuteRequest(async () => await _client.GetFileAsync(fileRequest.RecordId, fileRequest.FieldId, fileRequest.FileId));

    if (res.IsSuccessful is true)
    {
      return res.Value;
    }

    _logger.Error(
      "Unable to get file. {StatusCode} - {Message}.",
      res.StatusCode,
      res.Message
    );

    return null;
  }

  [ExcludeFromCodeCoverage]
  private async Task<ApiResponse<T>> ExecuteRequest<T>(Func<Task<ApiResponse<T>>> func, int retryLimit = 3)
  {
    ApiResponse<T> response;
    var retry = 1;

    do
    {

      response = await func();

      if (response.IsSuccessful is true)
      {
        return response;
      }

      _logger.Error(
        "Request was unsuccessful. {StatusCode} - {Message}. ({Attempt} of {AttemptLimit})",
        response.StatusCode,
        response.Message,
        retry,
        retryLimit
      );

      retry++;

      if (retry > retryLimit)
      {
        break;
      }

      var wait = 1000 * retry;

      _logger.Information(
        "Waiting {Wait}s before retrying request.",
        wait
      );

      Thread.Sleep(wait);

      _logger.Information(
        "Retrying request. {Attempt} of {AttemptLimit}",
        retry,
        retryLimit
      );
    } while (retry <= retryLimit);

    _logger.Error(
      "Request failed after {RetryLimit} attempts. {StatusCode} - {Message}.",
      retryLimit,
      response.StatusCode,
      response.Message
    );

    return response;
  }
}