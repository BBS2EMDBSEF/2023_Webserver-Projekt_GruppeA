using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using MimeKit;
using ServerAppSchule.Models;
using System.IO;
using System.IO.Compression;

namespace ServerAppSchule.Services
{
    public interface IFileService
    {
        List<FileSlim> GetdirsAndFiles(string path);
        string FileSizeFormater(double fileSizeInBytes);
        string DownloadPath(string usrname, string fileName);

    }
    public class FileService : IFileService
    {
        private static string _baseDir = "C:\\Users\\Public\\Documents\\Schule\\";

        private string GetFileType(string filename)
        {
            string[] split = filename.Split(".");
            return split[split.Length - 1];
        }
        private double GetFileSize(string filename)
        {
            FileInfo info = new FileInfo(filename);
            return info.Length;
        }
        private DateTime GetFileCreationDate(string filename)
        {
            FileInfo info = new FileInfo(filename);
            return info.CreationTime;
        }
        private DateTime GetFileLastModifiedDate(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.LastWriteTime;
        }

        public List<FileSlim> GetdirsAndFiles(string path)
        {
            List<FileSlim> all = new List<FileSlim>();
            foreach (string file in Directory.GetFiles(_baseDir + path))
            {
                all.Add(new FileSlim() {
                    Name = file.Replace(_baseDir + path + "\\", ""),
                    Type = GetFileType(file),
                    Size = GetFileSize(file),
                    CreationDate = GetFileCreationDate(file),
                    LastModified = GetFileLastModifiedDate(file)
                });
            }
            foreach (string dir in Directory.GetDirectories(_baseDir + path))
            {
                all.Add(new FileSlim()
                {
                    Name = dir.Replace(_baseDir + path + "\\", ""),
                    Type = "ordner",
                });
            }
            return all;
        }
        public string FileSizeFormater(double fileSizeInBytes)
        {
            const double kbThreshold = 1024;
            const double mbThreshold = 1024 * 1024;
            const double gbThreshold = 1024 * 1024 * 1024;


            if (fileSizeInBytes < 0)
            {
                return "Ungültige Größe";
            }
            else if (fileSizeInBytes < kbThreshold)
            {
                return fileSizeInBytes + " Bytes";
            }
            else if (fileSizeInBytes < mbThreshold)
            {
                double fileSizeInKB = (double)fileSizeInBytes / kbThreshold;
                return fileSizeInKB.ToString("0.##") + " KB";
            }
            else if (fileSizeInBytes < gbThreshold)
            {
                double fileSizeInMB = (double)fileSizeInBytes / mbThreshold;
                return fileSizeInMB.ToString("0.##") + " MB";
            }
            else
            {
                double fileSizeInGB = (double)fileSizeInBytes / gbThreshold;
                return fileSizeInGB.ToString("0.##") + " GB";
            }
        }
        public string DownloadPath(string usrname, string fileName)
        {
           return Path.Combine(_baseDir, usrname, fileName).ToString();
        }

    }

}

