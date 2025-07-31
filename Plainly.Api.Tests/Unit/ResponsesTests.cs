using Microsoft.AspNetCore.Http;

namespace Plainly.Api.Tests.Unit;

public class ResponsesTests
{
    [Fact]
    public void OkResponse_ShouldReturnStatus200()
    {
        var response = SuccessResponse.Ok().Build();
        response.Success.ShouldBeTrue();
        response.StatusCode.ShouldBe(StatusCodes.Status200OK);
    }

    [Fact]
    public void CreatedResponse_ShouldReturnStatus201()
    {
        var response = SuccessResponse.Created().Build();
        response.Success.ShouldBeTrue();
        response.StatusCode.ShouldBe(StatusCodes.Status201Created);
    }


    [Fact]
    public void InternalServerErrorResponse_ShouldReturnStatus500()
    {
        var response = ErrorResponse.InternalServerError().Build();
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status500InternalServerError);
    }

    [Fact]
    public void BadRequestResponse_ShouldReturnStatus400()
    {
        var response = ErrorResponse.BadRequest().Build();
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
    }

    [Fact]
    public void UnauthorizedResponse_ShouldReturnStatus401()
    {
        var response = ErrorResponse.Unauthorized().Build();
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status401Unauthorized);
    }

    [Fact]
    public void ForbiddenResponse_ShouldReturnStatus403()
    {
        var response = ErrorResponse.Forbidden().Build();
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status403Forbidden);
    }

    [Fact]
    public void NotFoundResponse_ShouldReturnStatus404()
    {
        var response = ErrorResponse.NotFound().Build();
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status404NotFound);
    }

    [Fact]
    public void ValidationErrorResponse_ShouldReturnStatus422()
    {
        var response = ErrorResponse.ValidationError().Build();
        response.Success.ShouldBeFalse();
        response.StatusCode.ShouldBe(StatusCodes.Status422UnprocessableEntity);
    }
}