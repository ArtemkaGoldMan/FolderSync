using Folderify.App.Services.Implementations;
using Xunit;
using System.IO;
using System;

namespace Folderify.Test
{
    public class DualLoggerTests
    {
        private readonly string _testLogPath = Path.Combine(Path.GetTempPath(), "test_log.txt");

        [Fact]
        public void Log_WritesToConsoleAndFile()
        {
            // Arrange
            var logger = new DualLogger(_testLogPath);
            const string testMessage = "Test log message";

            // Act
            logger.Log(testMessage);

            // Assert
            Assert.True(File.Exists(_testLogPath));
            var logContent = File.ReadAllText(_testLogPath);
            Assert.Contains(testMessage, logContent);
        }

        public void Dispose()
        {
            if (File.Exists(_testLogPath))
                File.Delete(_testLogPath);
        }
    }
}