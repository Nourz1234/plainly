using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Plainly.Frontend.Components;

public class AppComponent : ComponentBase
{
    [Inject]
    public required ISnackbar Snackbar { get; set; }

    [CascadingParameter]
    public required LoaderBoundary LoaderBoundary { get; set; }

    protected EventCallback Handle(Func<Task> handler)
    {
        return EventCallback.Factory.Create(this, () => RunWithHandling(handler));
    }

    protected EventCallback<T> Handle<T>(Func<T, Task> handler)
    {
        return EventCallback.Factory.Create<T>(this, (arg) => RunWithHandling(() => handler(arg)));
    }

    protected EventCallback<EditContext> Handle(Func<EditContext, Task> handler) => Handle<EditContext>(handler);
    protected EventCallback<MouseEventArgs> Handle(Func<MouseEventArgs, Task> handler) => Handle<MouseEventArgs>(handler);
    protected EventCallback<KeyboardEventArgs> Handle(Func<KeyboardEventArgs, Task> handler) => Handle<KeyboardEventArgs>(handler);


    public async Task RunWithHandling(Func<Task> handler)
    {
        LoaderBoundary.Show();
        try
        {
            await handler();
        }
        catch (Errors.AppError error)
        {
            Snackbar.Add(error.Message, Severity.Error);
        }
        finally
        {
            LoaderBoundary.Hide();
        }
    }
}