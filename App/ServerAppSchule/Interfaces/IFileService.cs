using Microsoft.AspNetCore.Components.Forms;
using ServerAppSchule.Models;

namespace ServerAppSchule.Interfaces
{
    public interface IFileService
    {
        Task<List<FileSlim>> GetdirsAndFiles(string path);
        string FileSizeFormater(double fileSizeInBytes);
        string DownloadPath(string usrname, string fileName);
        Task Upload(string usrName, IBrowserFile file);
        Task<string> PicToBase64Async(IBrowserFile input);
        Task Delete(string usrName, string fileName);
        string DownloadZipPath(string usrname, string dirName);
    }
}
