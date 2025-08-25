using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Plainly.Frontend.Errors;
using Plainly.Frontend.Services;

namespace Plainly.Frontend.Components;

public class AppComponent : ComponentBase, IHandleEvent
{
    [Inject, NotNull]
    public ISnackbar? Snackbar { get; set; }

    [Inject, NotNull]
    public CurrentUserService? CurrentUserService { get; set; }

    [CascadingParameter]
    public AppLoaderBoundary? LoaderBoundary { get; set; }

    // hook up events to allow global error handling and loading indicator
    Task IHandleEvent.HandleEventAsync(EventCallbackWorkItem callback, object? arg)
    {
        var task = callback.InvokeAsync(arg);
        var shouldAwaitTask = task.Status != TaskStatus.RanToCompletion &&
            task.Status != TaskStatus.Canceled;

        StateHasChanged();

        return shouldAwaitTask ?
            RunTaskWithHandlingAsync(task) :
            Task.CompletedTask;
    }

    protected override sealed void OnInitialized()
    {
    }

    protected override sealed Task OnInitializedAsync()
    {
        return RunWithHandlingAsync(RunLoadAsync, updateState: false); // do not update state, OnInitializedAsync should handle it
    }

    private Task RunLoadAsync()
    {
        OnLoad();
        return OnLoadAsync();
    }

    protected virtual void OnLoad() { }
    protected virtual Task OnLoadAsync() => Task.CompletedTask;


    private Task RunWithHandlingAsync(Func<Task> handler, bool updateState = true)
    {
        var task = handler();
        var shouldAwaitTask = task.Status != TaskStatus.RanToCompletion &&
            task.Status != TaskStatus.Canceled;

        // We always want to update the state here so that we render after the sync part of the handler
        StateHasChanged();

        return shouldAwaitTask ?
            RunTaskWithHandlingAsync(task, updateState) :
            Task.CompletedTask;
    }

    private async Task RunTaskWithHandlingAsync(Task task, bool updateState = true)
    {
        if (!task.IsCompleted)
            LoaderBoundary?.Show();
        try
        {
            await task;
        }
        catch (ApiError error)
        {
            Snackbar.Add<ErrorSnackbar>(
                new()
                {
                    [nameof(ErrorSnackbar.Message)] = error.Message,
                    [nameof(ErrorSnackbar.Errors)] = error.Errors!,
                    [nameof(ErrorSnackbar.TraceId)] = error.TraceId!,
                },
                Severity.Error,
                ConfigureErrorSnackbar
            );
        }
        catch (AuthError error)
        {
            await CurrentUserService.ClearCurrentUserAsync();
            Snackbar.Add<ErrorSnackbar>(
                new() { [nameof(ErrorSnackbar.Message)] = error.Message },
                Severity.Error,
                ConfigureErrorSnackbar
            );
        }
        catch (HttpRequestException)
        {
            Snackbar.Add<ErrorSnackbar>(
                new() { [nameof(ErrorSnackbar.Message)] = Messages.NetworkError },
                Severity.Error,
                ConfigureErrorSnackbar
            );
        }
        finally
        {
            LoaderBoundary?.Hide();
        }

        if (updateState)
            StateHasChanged();
    }

    protected virtual void ConfigureErrorSnackbar(SnackbarOptions options)
    {
        options.VisibleStateDuration = 10000;
    }

}