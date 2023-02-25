using OnspringAttachmentReporter.Services;

namespace OnspringAttachmentReporter.Models;

class Processor
{
  internal readonly int _appId;
  internal readonly OnspringService _onspringService;

  public Processor(Context context)
  {
    _appId = context.AppId;
    _onspringService = new OnspringService(context.ApiKey);
  }
}