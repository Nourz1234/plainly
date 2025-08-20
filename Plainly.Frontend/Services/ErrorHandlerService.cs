using MudBlazor;

namespace Plainly.Frontend.Services;


public class ErrorHandlerService(ISnackbar snackbar)
{
    public Func<Task> WithErrorHandling(Func<Task> handler) => async () =>
    {
        try
        {
            await handler();
        }
        catch (Errors.AppError error)
        {
            snackbar.Add(error.Message, Severity.Error);
        }
    };
}