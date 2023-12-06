using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using MimeKit;
using ServerAppSchule.Models;
using System.Buffers.Text;
using System.IO;
using System.IO.Compression;

namespace ServerAppSchule.Services
{
    public interface IFileService
    {
        List<FileSlim> GetdirsAndFiles(string path);
        string FileSizeFormater(double fileSizeInBytes);
        string DownloadPath(string usrname, string fileName);
        Task Upload(string usrName, IBrowserFile file);
        string PicToBase64(IBrowserFile input);
    }
    public class FileService : IFileService
    {
        #region private fields
        private static string _baseDir = "C:\\Users\\Nicklas\\OneDrive\\htdocs\\UploadTest\\Service\\";
        //private static string _baseDir = "/home/";
        #endregion
        #region private Methods
        /// <summary>
        /// Ermittelt die Datei Endung
        /// </summary>
        /// <param name="filename">Dateiname</param>
        /// <returns>Datei endung als string</returns>
        private string GetFileType(string filename)
        {
            string[] split = filename.Split(".");
            return split[split.Length - 1];
        }
        /// <summary>
        /// ermittelt die Dateigröße in Bytes
        /// </summary>
        /// <param name="filename">Dateiname</param>
        /// <returns>Dateigröße als double</returns>
        private double GetFileSize(string filename)
        {
            FileInfo info = new FileInfo(filename);
            return info.Length;
        }
        /// <summary>
        /// Ermittelt das Erstellungsdatum der Datei
        /// </summary>
        /// <param name="filename">Dateiname</param>
        /// <returns>Datum wann die Datei erstellt wurde</returns>
        private DateTime GetFileCreationDate(string filename)
        {
            FileInfo info = new FileInfo(filename);
            return info.CreationTime;
        }
        /// <summary>
        /// ermittlet das letzte Änderungsdatum der Datei
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns>Datum der letzten Änderung der Datei</returns>
        private DateTime GetFileLastModifiedDate(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            return fileInfo.LastWriteTime;
        }
        #endregion
        #region public Methods 
        /// <summary>
        /// Ermittelt alle Datein und Ordner in einem Verzeichnis
        /// </summary>
        /// <param name="path">Username</param>
        /// <returns>Liste aller Ordner und Dateien</returns>
        public List<FileSlim> GetdirsAndFiles(string path)
        {
            List<FileSlim> all = new List<FileSlim>();
            try
            {
                foreach (string file in Directory.GetFiles(_baseDir + path))
                {
                    all.Add(new FileSlim()
                    {
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
            catch
            {

            }
            return null;
        }
        /// <summary>
        /// Formatiert die Datei ins Passende Datei format
        /// </summary>
        /// <param name="fileSizeInBytes">Dateigröße in Bytes</param>
        /// <returns>formatierte Dateigröße als string</returns>
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
        /// <summary>
        /// gibt den Dateipfad im string format zurück
        /// </summary>
        /// <param name="usrname">benutzername</param>
        /// <param name="fileName">dateiname</param>
        /// <returns>Dateipfad als string</returns>
        public string DownloadPath(string usrname, string fileName)
        {
           return Path.Combine(_baseDir, usrname, fileName).ToString();
        }

        public async Task Upload(string usrName, IBrowserFile file)
        {
            string path = Path.Combine(_baseDir, usrName).ToString();
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                await file.OpenReadStream().CopyToAsync(fs);
            }

        }
        /// <summary>
        /// Wandelt ein Bild in ein Base64 String um
        /// </summary>
        /// <param name="input">Hochgeladenes Profilbild</param>
        /// <returns></returns>
        public string PicToBase64(IBrowserFile input)
        {
                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    input.OpenReadStream().CopyTo(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }
                string base64String = Convert.ToBase64String(fileBytes);
                return base64String;
        }

        #endregion
    }

}

