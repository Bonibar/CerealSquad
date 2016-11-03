using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealSquad.Downloaders
{
    interface IDownloader
    {
        Task RequireFile(string name, string localPath, Uri distantPath, bool Override = true);
        bool FileExist(string name);
        string GetFile(string name);
    }
}
