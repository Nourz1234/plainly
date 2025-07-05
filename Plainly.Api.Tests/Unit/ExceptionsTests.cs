using Plainly.Api.Exceptions;
using Shouldly;

namespace Plainly.Api.Tests.Unit;


public class ExceptionsTests
{
    [Fact]
    public void InternalServerErrorException_DefaultConstructor_ShouldSetDefaultMessage()
    {
        var exception = new InternalServerErrorException();
        exception.Message.ShouldBe(InternalServerErrorException.DefaultMessage);
    }

    [Fact]
    public void InternalServerErrorException_WithMessage_ShouldSetMessage()
    {
        var exception = new InternalServerErrorException("Test message");
        exception.Message.ShouldBe("Test message");
    }

    [Fact]
    public void InternalServerErrorException_WithInnerException_ShouldSetInnerException()
    {
        var innerException = new Exception("Inner exception message");
        var exception = new InternalServerErrorException("Test message", innerException);
        exception.Message.ShouldBe("Test message");
        exception.InnerException.ShouldBe(innerException);
    }


    [Fact]
    public void ForbiddenException_DefaultConstructor_ShouldSetDefaultMessage()
    {
        var exception = new ForbiddenException();
        exception.Message.ShouldBe(ForbiddenException.DefaultMessage);
    }

    [Fact]
    public void ForbiddenException_WithMessage_ShouldSetMessage()
    {
        var exception = new ForbiddenException("Test message");
        exception.Message.ShouldBe("Test message");
    }

    [Fact]
    public void ForbiddenException_WithInnerException_ShouldSetInnerException()
    {
        var innerException = new Exception("Inner exception message");
        var exception = new ForbiddenException("Test message", innerException);
        exception.Message.ShouldBe("Test message");
        exception.InnerException.ShouldBe(innerException);
    }


    [Fact]
    public void BadRequestException_DefaultConstructor_ShouldSetDefaultMessage()
    {
        var exception = new BadRequestException();
        exception.Message.ShouldBe(BadRequestException.DefaultMessage);
    }

    [Fact]
    public void BadRequestException_WithMessage_ShouldSetMessage()
    {
        var exception = new BadRequestException("Test message");
        exception.Message.ShouldBe("Test message");
    }

    [Fact]
    public void BadRequestException_WithInnerException_ShouldSetInnerException()
    {
        var innerException = new Exception("Inner exception message");
        var exception = new BadRequestException("Test message", innerException);
        exception.Message.ShouldBe("Test message");
        exception.InnerException.ShouldBe(innerException);
    }


    [Fact]
    public void NotFoundException_DefaultConstructor_ShouldSetDefaultMessage()
    {
        var exception = new NotFoundException();
        exception.Message.ShouldBe(NotFoundException.DefaultMessage);
    }

    [Fact]
    public void NotFoundException_WithMessage_ShouldSetMessage()
    {
        var exception = new NotFoundException("Test message");
        exception.Message.ShouldBe("Test message");
    }

    [Fact]
    public void NotFoundException_WithInnerException_ShouldSetInnerException()
    {
        var innerException = new Exception("Inner exception message");
        var exception = new NotFoundException("Test message", innerException);
        exception.Message.ShouldBe("Test message");
        exception.InnerException.ShouldBe(innerException);
    }


    [Fact]
    public void UnauthorizedException_DefaultConstructor_ShouldSetDefaultMessage()
    {
        var exception = new UnauthorizedException();
        exception.Message.ShouldBe(UnauthorizedException.DefaultMessage);
    }

    [Fact]
    public void UnauthorizedException_WithMessage_ShouldSetMessage()
    {
        var exception = new UnauthorizedException("Test message");
        exception.Message.ShouldBe("Test message");
    }

    [Fact]
    public void UnauthorizedException_WithInnerException_ShouldSetInnerException()
    {
        var innerException = new Exception("Inner exception message");
        var exception = new UnauthorizedException("Test message", innerException);
        exception.Message.ShouldBe("Test message");
        exception.InnerException.ShouldBe(innerException);
    }


    [Fact]
    public void ValidationException_DefaultConstructor_ShouldSetDefaultMessage()
    {
        var exception = new ValidationException();
        exception.Message.ShouldBe(ValidationException.DefaultMessage);
    }

    [Fact]
    public void ValidationException_WithMessage_ShouldSetMessage()
    {
        var exception = new ValidationException("Test message");
        exception.Message.ShouldBe("Test message");
    }

    [Fact]
    public void ValidationException_WithErrors_ShouldSetErrors()
    {
        var errors = new Dictionary<string, string[]> { ["field1"] = ["test"] };
        var exception = new ValidationException("Test message", errors);
        exception.Message.ShouldBe("Test message");
        exception.Errors.ShouldBeEquivalentTo(errors);
    }

    [Fact]
    public void ValidationException_WithInnerException_ShouldSetInnerException()
    {
        var innerException = new Exception("Inner exception message");
        var exception = new ValidationException("Test message", innerException);
        exception.Message.ShouldBe("Test message");
        exception.InnerException.ShouldBe(innerException);
    }

    [Fact]
    public void ValidationException_WithErrorsAndInnerException_ShouldSetErrorsAndInnerException()
    {
        var errors = new Dictionary<string, string[]> { ["field1"] = ["test"] };
        var innerException = new Exception("Inner exception message");
        var exception = new ValidationException("Test message", errors, innerException);
        exception.Message.ShouldBe("Test message");
        exception.InnerException.ShouldBe(innerException);
        exception.Errors.ShouldBeEquivalentTo(errors);
    }


}