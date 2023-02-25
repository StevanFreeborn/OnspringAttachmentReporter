using Onspring.API.SDK;

namespace OnspringAttachmentReporter.Services;

class OnspringService
{
  internal readonly string baseUrl = "https://api.onspring.com";
  internal readonly OnspringClient _client;

  public OnspringService(string? apiKey)
  {
    _client = new OnspringClient(baseUrl, apiKey);
  }
}