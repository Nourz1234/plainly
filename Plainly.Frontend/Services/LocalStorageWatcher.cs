using Microsoft.JSInterop;

namespace Plainly.Frontend.Services;

public class LocalStorageWatcher(IJSRuntime jsRuntime) : IDisposable
{
    private DotNetObjectReference<LocalStorageWatcher>? _ObjRef;

    public event Action<string, string?, string?>? StorageChanged;

    public async ValueTask StartWatchingAsync()
    {
        _ObjRef = DotNetObjectReference.Create(this);
        await jsRuntime.InvokeVoidAsync("localStorageHelper.registerStorageChanged", _ObjRef);
    }

    [JSInvokable]
    public void OnStorageChanged(string key, string? oldValue, string? newValue)
    {
        StorageChanged?.Invoke(key, oldValue, newValue);
    }

    public void Dispose()
    {
        _ObjRef?.Dispose();
    }
}
