namespace Folderify.App.Services.Interfaces
{
    public interface IScheduler : IDisposable
    {
        void Start();
        void Stop();
    }
}