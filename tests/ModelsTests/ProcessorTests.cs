using Moq;
using Onspring.API.SDK.Models;
using OnspringAttachmentReporter.Interfaces;

namespace OnspringAttachmentReporter.ModelsTests;

public class ProcessorTests
{
  [Fact]
  public void Processor_WhenConstructorIsCalledWithContext_ShouldReturnANewInstance()
  {
    var context = new Context("apiKey", 1);
    var service = new OnspringService(context);
    var processor = new Processor(service);
    processor.Should().NotBeNull();
    processor.Should().BeOfType<Processor>();
    processor._onspringService.Should().NotBeNull();
    processor._onspringService.Should().BeOfType<OnspringService>();
  }

  [Fact]
  public async Task GetFileFields_WhenCalledAndNoFileFieldsFound_ShouldReturnNull()
  {
    var context = new Context("apiKey", 1);
    var mockOnspringService = new Mock<IOnspringService>();
    mockOnspringService
    .Setup(m => m.GetAllFields(50))
    .ReturnsAsync((List<Field>?)null);

    var processor = new Processor(mockOnspringService.Object);
    var res = await processor.GetFileFields();
    res.Should().BeNull();
  }
}