using Folderify.App.Services.Interfaces;
using System.Security.Cryptography;

namespace Folderify.App.Services.Implementations
{
    public class FolderComparer : IFolderComparer
    {
        private readonly string _source;
        private readonly string _replica;

        public FolderComparer(string source, string replica)
        {
            _source = source;
            _replica = replica;
        }

        public IEnumerable<string> GetFilesToCopy()
        {
            var filesToCopy = new List<string>();
            try
            {
                foreach (var sourceFile in Directory.GetFiles(_source, "*", SearchOption.AllDirectories))
                {
                    try
                    {
                        var relativePath = Path.GetRelativePath(_source, sourceFile);
                        var replicaFile = Path.Combine(_replica, relativePath);

                        if (!File.Exists(replicaFile) || GetChecksum(sourceFile) != GetChecksum(replicaFile))
                        {
                            filesToCopy.Add(relativePath);
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            catch (Exception)
            {
                return Enumerable.Empty<string>();
            }
            return filesToCopy;
        }

        public IEnumerable<string> GetFilesToDelete()
        {
            var filesToDelete = new List<string>();
            try
            {
                foreach (var replicaFile in Directory.GetFiles(_replica, "*", SearchOption.AllDirectories))
                {
                    try
                    {
                        var relativePath = Path.GetRelativePath(_replica, replicaFile);
                        var sourceFile = Path.Combine(_source, relativePath);

                        if (!File.Exists(sourceFile))
                        {
                            filesToDelete.Add(relativePath);
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
            catch (Exception)
            {
                return Enumerable.Empty<string>();
            }
            return filesToDelete;
        }

        private string GetChecksum(string filePath)
        {
            try
            {
                using var md5 = MD5.Create();
                using var stream = File.OpenRead(filePath);
                var hash = md5.ComputeHash(stream);
                return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
            }
            catch (Exception)
            {
                // Return empty string if checksum can't be computed
                return string.Empty;
            }
        }
    }
}