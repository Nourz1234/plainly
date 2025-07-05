namespace Plainly.Api.Interfaces;

public interface IAction<TRequest, TResponse>
{
    public string InternalName { get; }
    public string DisplayName { get; }
    public Task<TResponse> Execute(TRequest request);
}