using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Plainly.Frontend.Errors;

namespace Plainly.Frontend.Components;

public class AppComponent : ComponentBase
{
    [Inject]
    public required IToastService ToastService { get; set; }

    [CascadingParameter]
    public required LoaderBoundary LoaderBoundary { get; set; }

    protected EventCallback Handle(Func<Task> handler)
    {
        return EventCallback.Factory.Create(this, async () =>
        {
            try
            {
                LoaderBoundary.Show();
                await handler();
            }
            catch (Exception ex)
            {
                if (!HandleError(ex))
                    throw;
            }
            finally
            {
                LoaderBoundary.Hide();
            }
        });
    }

    protected EventCallback<T> Handle<T>(Func<T, Task> handler)
    {
        return EventCallback.Factory.Create<T>(this, async (arg) =>
        {
            try
            {
                LoaderBoundary.Show();
                await handler(arg);
            }
            catch (Exception ex)
            {
                if (!HandleError(ex))
                    throw;
            }
            finally
            {
                LoaderBoundary.Hide();
            }
        });
    }

    protected EventCallback<EditContext> Handle(Func<EditContext, Task> handler) => Handle<EditContext>(handler);
    protected EventCallback<MouseEventArgs> Handle(Func<MouseEventArgs, Task> handler) => Handle<MouseEventArgs>(handler);
    protected EventCallback<KeyboardEventArgs> Handle(Func<KeyboardEventArgs, Task> handler) => Handle<KeyboardEventArgs>(handler);


    protected virtual bool HandleError(Exception ex)
    {
        if (ex is ApiError apiError)
        {
            ToastService.ShowError(apiError.Message);
            return true;
        }
        return false;
    }
}