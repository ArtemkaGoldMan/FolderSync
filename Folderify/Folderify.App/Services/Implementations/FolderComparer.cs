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
            foreach (var sourceFile in Directory.GetFiles(_source, "*", SearchOption.AllDirectories))
            {
                var relativePath = Path.GetRelativePath(_source, sourceFile);
                var replicaFile = Path.Combine(_replica, relativePath);

                if (!File.Exists(replicaFile) || GetChecksum(sourceFile) != GetChecksum(replicaFile))
                {
                    filesToCopy.Add(relativePath);
                }
            }
            return filesToCopy;
        }

        public IEnumerable<string> GetFilesToDelete()
        {
            var filesToDelete = new List<string>();
            foreach (var replicaFile in Directory.GetFiles(_replica, "*", SearchOption.AllDirectories))
            {
                var relativePath = Path.GetRelativePath(_replica, replicaFile);
                var sourceFile = Path.Combine(_source, relativePath);

                if (!File.Exists(sourceFile))
                {
                    filesToDelete.Add(relativePath);
                }
            }
            return filesToDelete;
        }

        private string GetChecksum(string filePath)
        {
            using var md5 = MD5.Create();
            using var stream = File.OpenRead(filePath);
            var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
        }
    }

}
