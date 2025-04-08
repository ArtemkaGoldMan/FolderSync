using Folderify.App.Services.Interfaces;

namespace Folderify.App.Services.Implementations
{
    public class DualLogger : ILogger
    {
        private readonly string _logFilePath;

        public DualLogger(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public void Log(string message)
        {
            try
            {
                var timestampedMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
                Console.WriteLine(timestampedMessage);
                File.AppendAllText(_logFilePath, timestampedMessage + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log: {ex.Message}");
            }
        }
    }
}