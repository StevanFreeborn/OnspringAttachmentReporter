namespace OnspringAttachmentReporter.ModelsTests;

public class ProcessorTests
{
  [Fact]
  public void Processor_WhenConstructorIsCalledWithContext_ShouldReturnANewInstance()
  {
    var context = new Context("apiKey", 1);
    var processor = new Processor(context);
    processor.Should().NotBeNull();
    processor.Should().BeOfType<Processor>();
    processor._appId.Should().Be(1);
    processor._onspringService.Should().NotBeNull();
    processor._onspringService.Should().BeOfType<OnspringService>();
  }
}