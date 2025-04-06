using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Folderify.App.Services.Interfaces
{
    public interface IFolderComparer
    {
        IEnumerable<string> GetFilesToCopy();
        IEnumerable<string> GetFilesToDelete();
    }
}
