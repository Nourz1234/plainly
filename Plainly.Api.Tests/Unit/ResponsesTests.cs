using FluentAssertions;
using Plainly.Shared.Responses;

namespace Plainly.Api.Tests.Unit;

public class ResponsesTests
{
    [Theory]
    [InlineData(200)]
    [InlineData(201)]
    [InlineData(204)]

    public void SuccessResponse_With2xxStatusCode_ShouldSetSuccessTrue(int statusCode)
    {
        var response = new SuccessResponse(statusCode) { Message = "Success" };
        response.Success.Should().BeTrue();
    }

    [Theory]
    [InlineData(200, new string[] { "Data1", "Data2" })]
    [InlineData(201, new string[] { "Data1", "Data2" })]

    public void SuccessResponse_With2xxStatusCodeAndData_ShouldSetSuccessTrueAndSetData(int statusCode, string[] data)
    {
        var response = new SuccessResponse<string[]>(statusCode) { Message = "Success", Data = data };
        response.Success.Should().BeTrue();
        response.Data.Should().BeEquivalentTo(data);
    }

    [Theory]
    [InlineData(301)]
    [InlineData(404)]
    [InlineData(500)]
    public void SuccessResponse_WithNon2xxStatusCode_ShouldThrowArgumentException(int statusCode)
    {
        var act = () => new SuccessResponse(statusCode) { Message = "Success" };
        act.Should().Throw<ArgumentException>().And.Message.Should().Be($"Success response status code must be in the 200s");
    }


    [Theory]
    [InlineData(400)]
    [InlineData(404)]
    [InlineData(500)]

    public void ErrorResponse_With4xxOr5xxStatusCode_ShouldSetSuccessFalse(int statusCode)
    {
        var response = new ErrorResponse(statusCode) { Message = "Error" };
        response.Success.Should().BeFalse();
    }

    [Theory]
    [InlineData(200)]
    [InlineData(201)]
    [InlineData(301)]
    public void ErrorResponse_WithNon4xxOr5xxStatusCode_ShouldThrowArgumentException(int statusCode)
    {
        var act = () => new ErrorResponse(statusCode) { Message = "Error" };
        act.Should().Throw<ArgumentException>().And.Message.Should().Be($"Error response status code must be in the 400s or 500s");
    }
}