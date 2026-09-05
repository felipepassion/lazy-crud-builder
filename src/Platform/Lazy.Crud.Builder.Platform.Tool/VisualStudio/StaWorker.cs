using System.Collections.Concurrent;

namespace Lazy.Crud.Builder.Platform.Tool.VisualStudio;

/// <summary>
/// A single STA thread on which every DTE call is executed. COM automation of Visual Studio
/// requires an apartment-threaded caller with a registered message filter.
/// </summary>
public sealed class StaWorker : IDisposable
{
    private readonly BlockingCollection<(Action work, TaskCompletionSource<bool> done)> _queue = new();
    private readonly Thread _thread;

    public StaWorker()
    {
        _thread = new Thread(Loop) { IsBackground = true, Name = "LazyCrud-DTE-STA" };
        if (OperatingSystem.IsWindows())
        {
            _thread.SetApartmentState(ApartmentState.STA);
        }
        _thread.Start();
    }

    private void Loop()
    {
        try { OleMessageFilter.Register(); } catch { /* filter is an optimization; calls still retry below */ }
        foreach (var (work, done) in _queue.GetConsumingEnumerable())
        {
            try { work(); done.TrySetResult(true); }
            catch (Exception ex) { done.TrySetException(ex); }
        }
        try { OleMessageFilter.Revoke(); } catch { /* process is ending */ }
    }

    public void Invoke(Action action, TimeSpan? timeout = null)
    {
        Invoke(() => { action(); return true; }, timeout);
    }

    public T Invoke<T>(Func<T> func, TimeSpan? timeout = null)
    {
        if (Thread.CurrentThread == _thread) return func();
        T result = default!;
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        _queue.Add((() => { result = func(); }, tcs));
        var t = timeout ?? TimeSpan.FromMinutes(10);
        if (!tcs.Task.Wait(t))
            throw new TimeoutException($"DTE call did not complete within {t.TotalSeconds:F0}s");
        return result;
    }

    public void Dispose()
    {
        _queue.CompleteAdding();
        _thread.Join(TimeSpan.FromSeconds(10));
    }
}
