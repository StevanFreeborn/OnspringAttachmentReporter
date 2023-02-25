using OnspringAttachmentReporter.Services;

namespace OnspringAttachmentReporter.Models;

class Processor
{
  private readonly int _appId;
  private readonly OnspringService _onspringService;

  public Processor(Context context)
  {
    _appId = context.AppId;
    _onspringService = new OnspringService(context.ApiKey);
  }
}