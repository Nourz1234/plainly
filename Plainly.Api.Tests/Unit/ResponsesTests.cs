using FluentAssertions;
using Plainly.Shared.Responses;

namespace Plainly.Api.Tests.Unit;

public class ResponsesTests
{
    [Theory]
    [InlineData(200)]
    [InlineData(201)]
    [InlineData(204)]

    public void TestSuccessResponse_ShouldSucceed(int statusCode)
    {
        var response = new SuccessResponse(statusCode) { Message = "Success" };
        response.Success.Should().BeTrue();
    }

    [Theory]
    [InlineData(301)]
    [InlineData(404)]
    [InlineData(500)]
    public void TestSuccessResponse_ShouldThrow(int statusCode)
    {
        var act = () => new SuccessResponse(statusCode) { Message = "Success" };
        act.Should().Throw<ArgumentException>().And.Message.Should().Be($"Success response status code must be in the 200s");
    }


    [Theory]
    [InlineData(400)]
    [InlineData(404)]
    [InlineData(500)]

    public void TestErrorResponse_ShouldSucceed(int statusCode)
    {
        var response = new ErrorResponse(statusCode) { Message = "Error" };
        response.Success.Should().BeFalse();
    }

    [Theory]
    [InlineData(200)]
    [InlineData(201)]
    [InlineData(301)]
    public void TestErrorResponse_ShouldThrow(int statusCode)
    {
        var act = () => new ErrorResponse(statusCode) { Message = "Error" };
        act.Should().Throw<ArgumentException>().And.Message.Should().Be($"Error response status code must be in the 400s or 500s");
    }
}