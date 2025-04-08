using Folderify.App.Services.Implementations;
using Folderify.App.Services.Interfaces;
using Moq;
using Xunit;
using System;
using System.Threading;

public class PeriodicSchedulerTests : IDisposable
{
    private readonly Mock<IFileSynchronizer> _mockSynchronizer = new();
    private readonly Mock<ILogger> _mockLogger = new();
    private PeriodicScheduler _scheduler;

    public PeriodicSchedulerTests()
    {
        _scheduler = new PeriodicScheduler(_mockSynchronizer.Object, _mockLogger.Object, 1);
    }

    [Fact]
    public void Start_InvokesSynchronizeAtInterval()
    {
        // Act
        _scheduler.Start();
        Thread.Sleep(1100); // Wait for 1.1 seconds (slightly more than the interval)
        _scheduler.Stop();

        // Assert
        _mockSynchronizer.Verify(s => s.Synchronize(), Times.AtLeastOnce());
        _mockLogger.Verify(l => l.Log(It.Is<string>(s => s.Contains("Starting"))));
    }

    [Fact]
    public void Stop_PreventsFurtherSynchronization()
    {
        // Arrange
        _scheduler.Start();
        _scheduler.Stop();
        _mockSynchronizer.Invocations.Clear(); // Reset call count

        // Act
        Thread.Sleep(1100); // Wait for 1.1 seconds

        // Assert
        _mockSynchronizer.Verify(s => s.Synchronize(), Times.Never());
        _mockLogger.Verify(l => l.Log(It.Is<string>(s => s.Contains("stopped", StringComparison.OrdinalIgnoreCase))));
    }

    public void Dispose()
    {
        _scheduler.Dispose();
    }
}