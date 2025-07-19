# Adding an Action

### 1. Define `IAction`:
```csharp
class TestAction : IAction<TestRequest, TestDTO>
{
    public string DisplayName => "Test";
    public string InternalName => "TestAction";
    public Scopes[] RequiredScopes => [Scopes.PerformAction]; // or empty array if public
}
```

### 2-A. Define the request for the action:

This just lets us define what data we want and from where:
```csharp
// prefer record
record TestRequest(TestRouteParams RouteParams, TestQuery Query, TestForm Form);
```

- You should define the parameters as needed, eg omit url query if not used
- Each source should then be defined with it's validators

### 2-B. Define each data source for the request:

There are a couple of rules to follow to avoid getting a bad request error instead of a proper validation error:

1. Must define the data structure with a parameter-less constructor
2. Avoid using validation annotations (eg. [Required]), instead use default values, and handle all validation logic in the validator class.

Example of `TestForm`:
```csharp
// parameter-less constructor
record TestForm()
{
    // No annotations for required field
    string Field { get; init; } = String.Empty; // Default value
}

// All validation should be handled here
class TestFormValidator : AbstractValidator<TestForm>
{
    public RequestFormValidator()
    {
        RuleFor(x => x.Field).NotEmpty(); // Required
    }
}
```

Same for `TestRouteParams` and `TestQuery`. (if used)

### 3. Define the response for the action:

```csharp
record TestDTO(string Result);
```

