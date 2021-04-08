using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SimpleMemory.Helper;
using SimpleMemory.CacheManager;

/// <summary>
/// Base class for implementing a long running <see cref="IHostedService"/>.
/// Details at https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-5.0&tabs=visual-studio
/// </summary>
public abstract class BackgroundService : IHostedService, IDisposable
{
    private int executionCount = 0;
    private Timer _timer;
    private TimeHelper timeHelper;

    public BackgroundService()
    {
        this.timeHelper = new TimeHelper();
    }

    public Task StartAsync(CancellationToken stoppingToken)
    {
        _timer = new Timer(CheckFlushableCaches, null, TimeSpan.FromHours(1), 
            TimeSpan.FromHours(1));

        return Task.CompletedTask;
    }

    private void CheckFlushableCaches(object state)
    {
        var nowMinusOne = timeHelper.CreateLocalTimestamp().PlusMinutes(-60);

        CacheAccessSingleton.Instance.FlushOld(nowMinusOne);
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {

        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }
}