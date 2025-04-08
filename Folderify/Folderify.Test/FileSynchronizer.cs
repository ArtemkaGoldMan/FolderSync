using Folderify.App.Services.Implementations;
using Folderify.App.Services.Interfaces;
using Moq;
using Xunit;

namespace Folderify.Test
{
    public class FileSynchronizerTests : IDisposable
    {
        private readonly string _testSourceDir = Path.Combine(Path.GetTempPath(), "SyncSource");
        private readonly string _testReplicaDir = Path.Combine(Path.GetTempPath(), "SyncReplica");
        private readonly Mock<ILogger> _mockLogger = new();

        public FileSynchronizerTests()
        {
            Directory.CreateDirectory(_testSourceDir);
            Directory.CreateDirectory(_testReplicaDir);
        }

        [Fact]
        public void Synchronize_CopiesNewFileToReplica()
        {
            // Arrange
            var testFile = Path.Combine(_testSourceDir, "newfile.txt");
            File.WriteAllText(testFile, "Test content");

            var comparer = new FolderComparer(_testSourceDir, _testReplicaDir);
            var synchronizer = new FileSynchronizer(_testSourceDir, _testReplicaDir, comparer, _mockLogger.Object);

            // Act
            synchronizer.Synchronize();

            // Assert
            var replicaFile = Path.Combine(_testReplicaDir, "newfile.txt");
            Assert.True(File.Exists(replicaFile));
            _mockLogger.Verify(l => l.Log(It.Is<string>(s => s.Contains("Copied/Updated"))));
        }

        public void Dispose()
        {
            Directory.Delete(_testSourceDir, true);
            Directory.Delete(_testReplicaDir, true);
        }
    }
}