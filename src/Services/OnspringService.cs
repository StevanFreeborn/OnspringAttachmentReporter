using Onspring.API.SDK;

namespace OnspringAttachmentReporter.Services;

class OnspringService
{
  private const string baseUrl = "https://api.onspring.com";
  private readonly OnspringClient _client;

  public OnspringService(string apiKey)
  {
    _client = new OnspringClient(baseUrl, apiKey);
  }
}