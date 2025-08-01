using Microsoft.AspNetCore.Http;

namespace Plainly.Api.Tests.Unit;

public class ResponsesTests
{
    [Fact]
    public void OkResponse_ShouldSetStatus200()
    {
        var response = SuccessResponse.Ok().Build();
        response.Success.ShouldBeTrue();
        response.StatusCode.ShouldBe(StatusCodes.Status200OK);
    }

    [Fact]
    public void CreatedResponse_ShouldSetStatus201()
    {
        var response = SuccessResponse.Created().Build();
        response.Success.ShouldBeTrue();
        response.StatusCode.ShouldBe(StatusCodes.Status201Created);
    }


    [Fact]
    public void InternalServerErrorResponse_ShouldSetStatus500()
    {
        var response = ErrorResponse.InternalServerError().Build("Test");
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public void BadRequestResponse_ShouldSetStatus400()
    {
        var response = ErrorResponse.BadRequest().Build("Test");
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public void UnauthorizedResponse_ShouldSetStatus401()
    {
        var response = ErrorResponse.Unauthorized().Build("Test");
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public void ForbiddenResponse_ShouldSetStatus403()
    {
        var response = ErrorResponse.Forbidden().Build("Test");
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status403Forbidden);
    }

    [Fact]
    public void NotFoundResponse_ShouldSetStatus404()
    {
        var response = ErrorResponse.NotFound().Build("Test");
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public void ValidationErrorResponse_ShouldSetStatus422()
    {
        var response = ErrorResponse.ValidationError().Build("Test");
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status422UnprocessableEntity);
    }
}