using Folderify.App.Services.Interfaces;

namespace Folderify.App.Services.Implementations
{
    public class PeriodicScheduler : IScheduler
    {
        private readonly IFileSynchronizer _synchronizer;
        private readonly ILogger _logger;
        private readonly int _interval;
        private Timer? _timer;

        public PeriodicScheduler(IFileSynchronizer synchronizer, ILogger logger, int interval)
        {
            _synchronizer = synchronizer;
            _logger = logger;
            _interval = interval * 1000;
        }

        public void Start()
        {
            try
            {
                _logger.Log("Starting folder synchronization scheduler...");
                _timer = new Timer(_ => 
                {
                    try
                    {
                        _synchronizer.Synchronize();
                    }
                    catch (Exception ex)
                    {
                        _logger.Log($"Error during synchronization: {ex.Message}");
                    }
                }, null, 0, _interval);
                Console.ReadLine(); // keep app running
            }
            catch (Exception ex)
            {
                _logger.Log($"Scheduler startup error: {ex.Message}");
            }
        }
    }
}