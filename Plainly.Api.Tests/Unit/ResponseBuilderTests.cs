using Microsoft.AspNetCore.Http;

namespace Plainly.Api.Tests.Unit;

public class ResponseBuilderTests
{
    [Fact]
    public void SuccessResponse_Default_ShouldSetDefaultMessage()
    {
        var response = SuccessResponse.Ok().Build();
        response.Success.ShouldBeTrue();
        response.StatusCode.ShouldBe(StatusCodes.Status200OK);
        response.Message.ShouldBe(Messages.Success);
    }

    [Fact]
    public void SuccessResponse_WithMessage_ShouldSetMessage()
    {
        var message = "Test message";
        var response = SuccessResponse.Ok().WithMessage(message).Build();
        response.Success.ShouldBeTrue();
        response.StatusCode.ShouldBe(StatusCodes.Status200OK);
        response.Message.ShouldBe(message);
    }

    [Fact]
    public void SuccessResponse_WithData_ShouldSetData()
    {
        var data = new { Id = 1, Name = "Test" };
        var response = SuccessResponse.Ok().Build(data);
        response.Success.ShouldBeTrue();
        response.StatusCode.ShouldBe(StatusCodes.Status200OK);
        response.Message.ShouldBe(Messages.Success);
        response.Data.ShouldBe(data);
    }

    [Fact]
    public void SuccessResponse_WithMessageAndData_ShouldSetData()
    {
        var message = "Test message";
        var data = new { Id = 1, Name = "Test" };
        var response = SuccessResponse.Ok().WithMessage(message).Build(data);
        response.Success.ShouldBeTrue();
        response.StatusCode.ShouldBe(StatusCodes.Status200OK);
        response.Message.ShouldBe(message);
        response.Data.ShouldBe(data);
    }

    [Fact]
    public void ErrorResponse_Default_ShouldSetDefaultMessage()
    {
        var traceId = "test-trace-id";
        var response = ErrorResponse.BadRequest().Build(traceId);
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        response.Message.ShouldBe(Messages.BadRequest);
        response.TraceId.ShouldBe(traceId);
    }

    [Fact]
    public void ErrorResponse_WithMessage_ShouldSetMessage()
    {
        var traceId = "test-trace-id";
        var message = "Test message";
        var response = ErrorResponse.BadRequest().WithMessage(message).Build(traceId);
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        response.Message.ShouldBe(message);
        response.TraceId.ShouldBe(traceId);
    }

    [Fact]
    public void ErrorResponse_WithErrors_ShouldSetErrors()
    {
        var traceId = "test-trace-id";
        var errors = new ErrorDetail[] { new("test", "test") };
        var response = ErrorResponse.BadRequest().WithErrors(errors).Build(traceId);
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
        response.Message.ShouldBe(Messages.BadRequest);
        response.Errors.ShouldBe(errors);
        response.TraceId.ShouldBe(traceId);
    }
}