using FluentAssertions;
using Plainly.Api.Exceptions;

namespace Plainly.Api.Tests.Unit;


public class ExceptionsTests
{
    [Fact]
    public void TestInternalServerErrorException_DefaultMessage()
    {
        var exception = new InternalServerErrorException();
        exception.Message.Should().Be(InternalServerErrorException.DefaultMessage);
    }

    [Fact]
    public void TestInternalServerErrorException_WithMessage()
    {
        var exception = new InternalServerErrorException("Test message");
        exception.Message.Should().Be("Test message");
    }

    [Fact]
    public void TestInternalServerErrorException_WithInnerException()
    {
        var innerException = new Exception("Inner exception message");
        var exception = new InternalServerErrorException("Test message", innerException);
        exception.Message.Should().Be("Test message");
        exception.InnerException.Should().Be(innerException);
    }


    [Fact]
    public void TestForbiddenException_DefaultMessage()
    {
        var exception = new ForbiddenException();
        exception.Message.Should().Be(ForbiddenException.DefaultMessage);
    }

    [Fact]
    public void TestForbiddenException_WithMessage()
    {
        var exception = new ForbiddenException("Test message");
        exception.Message.Should().Be("Test message");
    }

    [Fact]
    public void TestForbiddenException_WithInnerException()
    {
        var innerException = new Exception("Inner exception message");
        var exception = new ForbiddenException("Test message", innerException);
        exception.Message.Should().Be("Test message");
        exception.InnerException.Should().Be(innerException);
    }


    [Fact]
    public void TestBadRequestException_DefaultMessage()
    {
        var exception = new BadRequestException();
        exception.Message.Should().Be(BadRequestException.DefaultMessage);
    }

    [Fact]
    public void TestBadRequestException_WithMessage()
    {
        var exception = new BadRequestException("Test message");
        exception.Message.Should().Be("Test message");
    }

    [Fact]
    public void TestBadRequestException_WithInnerException()
    {
        var innerException = new Exception("Inner exception message");
        var exception = new BadRequestException("Test message", innerException);
        exception.Message.Should().Be("Test message");
        exception.InnerException.Should().Be(innerException);
    }


    [Fact]
    public void TestNotFoundException_DefaultMessage()
    {
        var exception = new NotFoundException();
        exception.Message.Should().Be(NotFoundException.DefaultMessage);
    }

    [Fact]
    public void TestNotFoundException_WithMessage()
    {
        var exception = new NotFoundException("Test message");
        exception.Message.Should().Be("Test message");
    }

    [Fact]
    public void TestNotFoundException_WithInnerException()
    {
        var innerException = new Exception("Inner exception message");
        var exception = new NotFoundException("Test message", innerException);
        exception.Message.Should().Be("Test message");
        exception.InnerException.Should().Be(innerException);
    }


    [Fact]
    public void TestUnauthorizedException_DefaultMessage()
    {
        var exception = new UnauthorizedException();
        exception.Message.Should().Be(UnauthorizedException.DefaultMessage);
    }

    [Fact]
    public void TestUnauthorizedException_WithMessage()
    {
        var exception = new UnauthorizedException("Test message");
        exception.Message.Should().Be("Test message");
    }

    [Fact]
    public void TestUnauthorizedException_WithInnerException()
    {
        var innerException = new Exception("Inner exception message");
        var exception = new UnauthorizedException("Test message", innerException);
        exception.Message.Should().Be("Test message");
        exception.InnerException.Should().Be(innerException);
    }


    [Fact]
    public void TestValidationException_DefaultMessage()
    {
        var exception = new ValidationException();
        exception.Message.Should().Be(ValidationException.DefaultMessage);
    }

    [Fact]
    public void TestValidationException_WithMessage()
    {
        var exception = new ValidationException("Test message");
        exception.Message.Should().Be("Test message");
    }

    [Fact]
    public void TestValidationException_WithErrors()
    {
        var errors = new Dictionary<string, string[]> { ["field1"] = ["test"] };
        var exception = new ValidationException("Test message", errors);
        exception.Message.Should().Be("Test message");
        exception.Errors.Should().BeEquivalentTo(errors);
    }

    [Fact]
    public void TestValidationException_WithInnerException()
    {
        var innerException = new Exception("Inner exception message");
        var exception = new ValidationException("Test message", innerException);
        exception.Message.Should().Be("Test message");
        exception.InnerException.Should().Be(innerException);
    }

    [Fact]
    public void TestValidationException_WithErrorsAndInnerException()
    {
        var errors = new Dictionary<string, string[]> { ["field1"] = ["test"] };
        var innerException = new Exception("Inner exception message");
        var exception = new ValidationException("Test message", errors, innerException);
        exception.Message.Should().Be("Test message");
        exception.InnerException.Should().Be(innerException);
        exception.Errors.Should().BeEquivalentTo(errors);
    }


}