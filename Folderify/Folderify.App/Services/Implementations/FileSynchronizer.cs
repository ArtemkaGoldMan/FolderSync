using Folderify.App.Services.Interfaces;

namespace Folderify.App.Services.Implementations
{
    public class FileSynchronizer : IFileSynchronizer
    {
        private readonly string _source;
        private readonly string _replica;
        private readonly IFolderComparer _comparer;
        private readonly ILogger _logger;

        public FileSynchronizer(string source, string replica, IFolderComparer comparer, ILogger logger)
        {
            _source = source;
            _replica = replica;
            _comparer = comparer;
            _logger = logger;
        }

        public void Synchronize()
        {
            try
            {
                foreach (var file in _comparer.GetFilesToCopy())
                {
                    try
                    {
                        var sourcePath = Path.Combine(_source, file);
                        var replicaPath = Path.Combine(_replica, file);
                        Directory.CreateDirectory(Path.GetDirectoryName(replicaPath)!);
                        File.Copy(sourcePath, replicaPath, true);
                        _logger.Log($"Copied/Updated: {file}");
                    }
                    catch (Exception ex)
                    {
                        _logger.Log($"Error copying file {file}: {ex.Message}");
                    }
                }

                foreach (var file in _comparer.GetFilesToDelete())
                {
                    try
                    {
                        var replicaPath = Path.Combine(_replica, file);
                        File.Delete(replicaPath);
                        _logger.Log($"Deleted: {file}");
                    }
                    catch (Exception ex)
                    {
                        _logger.Log($"Error deleting file {file}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Synchronization error: {ex.Message}");
            }
        }
    }
}