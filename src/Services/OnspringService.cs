using Onspring.API.SDK;
using Onspring.API.SDK.Models;
using OnspringAttachmentReporter.Interfaces;
using OnspringAttachmentReporter.Models;
using Serilog;

namespace OnspringAttachmentReporter.Services;

class OnspringService : IOnspringService
{
  internal readonly string baseUrl = "https://api.onspring.com";
  internal readonly OnspringClient _client;
  internal readonly Context _context;

  public OnspringService(Context context)
  {
    _context = context;
    _client = new OnspringClient(baseUrl, context.ApiKey);
  }

  public async Task<List<Field>?> GetAllFields(int pageSize = 50)
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
        Log.Error(
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

  internal async static Task<TResult> ExecuteRequest<TResult>(Func<Task<TResult>> func, int retryLimit = 3)
  {
    var retry = 0;

    while (retry < retryLimit)
    {
      try
      {
        return await func();
      }
      catch (Exception ex)
      {
        Log.Error(
          "Unable to execute request. {ExceptionMessage} ({Attempt} of {AttemptLimit})",
          ex.Message,
          retry,
          retryLimit
        );

        retry++;

        if (retry == retryLimit)
        {
          throw;
        }

        var wait = 1000 * retry;

        Log.Information(
          "Waiting {Wait}s before retrying request.",
          wait
        );

        Thread.Sleep(wait);

        Log.Information(
          "Retrying request.' {Attempt} of {AttemptLimit}",
          retry,
          retryLimit
        );
      }
    }

    throw new Exception($"Unable to execute request after {retry} retries.");
  }
}