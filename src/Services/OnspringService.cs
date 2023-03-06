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

  public async Task<List<Field>> GetAllFields()
  {
    try
    {
      var fields = new List<Field>();
      var totalPages = 1;
      var pagingRequest = new PagingRequest(1, 50);
      var currentPage = pagingRequest.PageNumber;

      var options = new ProgressBarOptions
      {
        ForegroundColor = ConsoleColor.DarkBlue,
        ProgressCharacter = '─',
        ShowEstimatedDuration = false,
      };

      using var progressBar = new ProgressBar(
        totalPages,
        "Retrieving file fields...",
        options
      );

      do
      {
        progressBar.Tick($"Retrieving file fields from page {currentPage} of fields.");

        var res = await ExecuteRequest(
          async () => await _client.GetFieldsForAppAsync(
            _context.AppId,
            pagingRequest
          )
        );

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

        progressBar.Tick($"Retrieved file fields from page {currentPage} of fields.");

        pagingRequest.PageNumber++;
        currentPage = pagingRequest.PageNumber;
      } while (currentPage <= totalPages);

      progressBar.Tick("Finished retrieving file fields.");

      return fields;
    }
    catch (Exception ex)
    {
      _logger.Error(
        ex,
        "Unable to get fields."
      );

      return new List<Field>();
    }
  }

  public async Task<GetPagedRecordsResponse?> GetAPageOfRecords(List<int> fileFields, PagingRequest pagingRequest)
  {
    try
    {
      var request = new GetRecordsByAppRequest
      {
        AppId = _context.AppId,
        PagingRequest = pagingRequest,
        FieldIds = fileFields
      };

      var res = await ExecuteRequest(
        async () => await _client.GetRecordsForAppAsync(request)
      );

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
    catch (Exception ex)
    {
      _logger.Error(
        ex,
        "Unable to get records."
      );

      return null;
    }
  }

  public async Task<GetFileResponse?> GetFile(FileInfoRequest fileRequest)
  {
    try
    {
      var res = await ExecuteRequest(
        async () => await _client.GetFileAsync(
          fileRequest.RecordId,
          fileRequest.FieldId,
          fileRequest.FileId
        )
      );

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
    catch (Exception ex)
    {
      _logger.Error(
        ex,
        "Unable to get file: {@FileRequest}.",
        fileRequest
      );

      return null;
    }
  }

  [ExcludeFromCodeCoverage]
  private async Task<ApiResponse<T>> ExecuteRequest<T>(Func<Task<ApiResponse<T>>> func, int retry = 1)
  {
    ApiResponse<T> response;
    var retryLimit = 3;

    try
    {
      do
      {
        response = await func();

        if (response.IsSuccessful is true)
        {
          return response;
        }

        _logger.Warning(
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

        await Wait(retry);
      } while (retry <= retryLimit);
    }
    catch (Exception ex) when (ex is HttpRequestException || ex is TaskCanceledException)
    {
      _logger.Error(
        ex,
        "Request failed. ({Attempt} of {AttemptLimit})",
        retry,
        retryLimit
      );

      retry++;

      if (retry > retryLimit)
      {
        throw;
      }

      await Wait(retry);

      return await ExecuteRequest(func, retry);
    }

    _logger.Error(
      "Request failed after {RetryLimit} attempts. {StatusCode} - {Message}.",
      retryLimit,
      response.StatusCode,
      response.Message
    );

    return response;
  }

  [ExcludeFromCodeCoverage]
  private async Task Wait(int retryAttempt)
  {
    var wait = 1000 * retryAttempt;

    _logger.Debug(
      "Waiting {Wait}s before retrying request.",
      wait
    );

    await Task.Delay(wait);

    _logger.Debug(
      "Retrying request. {Attempt} of {AttemptLimit}",
      retryAttempt,
      3
    );
  }
}