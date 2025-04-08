using Folderify.App.Services.Implementations;
using Xunit;

namespace Folderify.Test
{
    public class FolderComparerTests
    {
        private readonly string _testSourceDir = Path.Combine(Path.GetTempPath(), "TestSource");
        private readonly string _testReplicaDir = Path.Combine(Path.GetTempPath(), "TestReplica");

        public FolderComparerTests()
        {
            // Setup test directories
            Directory.CreateDirectory(_testSourceDir);
            Directory.CreateDirectory(_testReplicaDir);
        }

        [Fact]
        public void GetFilesToCopy_WhenFileMissingInReplica_ReturnsFile()
        {
            // Arrange
            File.WriteAllText(Path.Combine(_testSourceDir, "test.txt"), "Hello");
            var comparer = new FolderComparer(_testSourceDir, _testReplicaDir);

            // Act
            var filesToCopy = comparer.GetFilesToCopy();

            // Assert
            Assert.Contains("test.txt", filesToCopy);
        }

        [Fact]
        public void GetFilesToDelete_WhenFileMissingInSource_ReturnsFile()
        {
            // Arrange
            File.WriteAllText(Path.Combine(_testReplicaDir, "extra.txt"), "Extra");
            var comparer = new FolderComparer(_testSourceDir, _testReplicaDir);

            // Act
            var filesToDelete = comparer.GetFilesToDelete();

            // Assert
            Assert.Contains("extra.txt", filesToDelete);
        }

        public void Dispose()
        {
            // Cleanup
            Directory.Delete(_testSourceDir, true);
            Directory.Delete(_testReplicaDir, true);
        }
    }
}