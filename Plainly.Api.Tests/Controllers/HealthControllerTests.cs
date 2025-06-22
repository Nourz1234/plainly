using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace Plainly.Api.Tests.Controllers;

[ExcludeFromCodeCoverage]
public class HealthControllerTests
{
    [Fact]
    public void TestHealth_ShouldReturnHealthy()
    {
        var controller = new Api.Controllers.HealthController();

        var result = controller.Get();
        result.Status.Should().Be("Healthy");
    }
}