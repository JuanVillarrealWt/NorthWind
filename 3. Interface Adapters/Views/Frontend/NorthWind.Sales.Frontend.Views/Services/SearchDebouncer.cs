namespace NorthWind.Sales.Frontend.Views.Services;

public sealed class SearchDebouncer : IDisposable
{
    private CancellationTokenSource? _pendingSearch;

    public async Task RunAsync(Func<Task> search, int delayMilliseconds = 400)
    {
        _pendingSearch?.Cancel();
        var pendingSearch = new CancellationTokenSource();
        _pendingSearch = pendingSearch;

        try
        {
            await Task.Delay(delayMilliseconds, pendingSearch.Token);
            if (!pendingSearch.IsCancellationRequested)
            {
                await search();
            }
        }
        catch (OperationCanceledException) when (pendingSearch.IsCancellationRequested)
        {
        }
        finally
        {
            if (ReferenceEquals(_pendingSearch, pendingSearch))
            {
                _pendingSearch = null;
            }

            pendingSearch.Dispose();
        }
    }

    public void CancelPending() => _pendingSearch?.Cancel();

    public void Dispose()
    {
        _pendingSearch?.Cancel();
        _pendingSearch?.Dispose();
        _pendingSearch = null;
    }
}
