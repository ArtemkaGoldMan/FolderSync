namespace Folderify.App.Services.Interfaces
{
    public interface IFolderComparer
    {
        IEnumerable<string> GetFilesToCopy();
        IEnumerable<string> GetFilesToDelete();
    }
}
