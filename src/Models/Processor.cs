using Onspring.API.SDK.Enums;
using Onspring.API.SDK.Models;
using OnspringAttachmentReporter.Interfaces;

namespace OnspringAttachmentReporter.Models;

class Processor : IProcessor
{
  internal readonly int _appId;
  internal readonly IOnspringService _onspringService;

  public Processor(IOnspringService onspringService)
  {
    _onspringService = onspringService;
  }

  public async Task<List<Field>?> GetFileFields()
  {
    var fields = await _onspringService.GetAllFields();

    if (fields is null)
    {
      return null;
    }

    return fields
    .Where(f => f.Type == FieldType.Attachment || f.Type == FieldType.Image)
    .ToList();
  }
}