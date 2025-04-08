using Folderify.App.Services.Implementations;
using Folderify.App.Services.Interfaces;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length != 4)
            {
                Console.WriteLine("Usage: FolderSync <source> <replica> <intervalInSeconds> <logFile>");
                return;
            }

            var source = args[0];
            var replica = args[1];
            if (!int.TryParse(args[2], out int interval))
            {
                Console.WriteLine("Error: Interval must be a valid integer");
                return;
            }
            var logFile = args[3];

            ILogger logger = new DualLogger(logFile);
            IFolderComparer comparer = new FolderComparer(source, replica);
            IFileSynchronizer synchronizer = new FileSynchronizer(source, replica, comparer, logger);
            IScheduler scheduler = new PeriodicScheduler(synchronizer, logger, interval);

            scheduler.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}