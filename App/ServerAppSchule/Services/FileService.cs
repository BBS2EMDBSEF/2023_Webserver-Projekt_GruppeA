using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ServerAppSchule.Models;
using System.IO.Compression;
using System.Security.AccessControl;

namespace ServerAppSchule.Services
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
    public class FileService : IFileService
    {
        #region private fields
        private static string _baseDir = @"C:\Users\Nicklas\.vsRepos\2023_Webserver-Projekt_GruppeA\App\ServerAppSchule\";
        //private static string _baseDir = "/home/";
        private IJSRuntime _jsRuntime;
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
        /// <summary>
        /// Gibt die Anzahl der Dateien mit dem selben Namen zurück
        /// </summary>
        /// <param name="name">Dateiname mit Pfad</param>
        /// <returns>Anzahl der Dateien mit gleichen namen</returns>
        private int GetSameNameFilesCount(string name)
        {
            int count = 0;
            while(File.Exists(name + "(" + count+1 + ")"))
            {
                count++;
            }
            return count;
        }
        /// <summary>
        /// Erstellt ein Zip Archiv
        /// </summary>
        /// <param name="usrName">Benutzername</param>
        /// <param name="dirName">Ordner name</param>
        /// <returns>Pfad zur Zip Datei</returns>
        private string CreateZip(string usrName, string dirName)
        {
            string path = Path.Combine(_baseDir, usrName, dirName).ToString();
            string zipPath = Path.Combine(_baseDir, usrName, dirName + ".zip").ToString();
            ZipFile.CreateFromDirectory(path, zipPath);
            return zipPath;
        }
        #endregion
        #region public Methods 
        /// <summary>
        /// Ermittelt alle Datein und Ordner in einem Verzeichnis
        /// </summary>
        /// <param name="path">Username</param>
        /// <returns>Liste aller Ordner und Dateien</returns>
        public async Task<List<FileSlim>> GetdirsAndFiles(string path)
        {
            List<FileSlim> all = new List<FileSlim>();
            try
            {
                foreach (string file in Directory.GetFiles(_baseDir + path))
                {
                    if (!file.Contains("/.")) {
                        all.Add(new FileSlim()
                        {
                            Name = file.Replace(_baseDir + path + "/", ""),
                            Type = GetFileType(file),
                            Size = GetFileSize(file),
                            CreationDate = GetFileCreationDate(file),
                            LastModified = GetFileLastModifiedDate(file)
                        });
                    }
                   
                }
                foreach (string dir in Directory.GetDirectories(_baseDir + path))
                {
                    all.Add(new FileSlim()
                    {
                        Name = dir.Replace(_baseDir + path + "/", ""),
                        Type = "ordner",
                    });
                }
                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(_baseDir + path);
                    DirectorySecurity directorySecurity = directoryInfo.GetAccessControl();
                    directorySecurity.AddAccessRule(new FileSystemAccessRule("Users", 
                        FileSystemRights.Write,
                        InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, 
                        PropagationFlags.None, 
                        AccessControlType.Allow));
                    directoryInfo.SetAccessControl(directorySecurity);
                }
                catch (Exception ex)
                {
                    _jsRuntime.InvokeVoidAsync("alert" + ex.Message);
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

        public string DownloadZipPath(string usrname, string dirName)
        {
            return CreateZip(usrname, dirName);
        }
        /// <summary>
        /// Lädt eine Datei hoch
        /// </summary>
        /// <param name="usrName">Benutzername</param>
        /// <param name="file">Datei die Hochgeladen werden soll</param>
        /// <returns></returns>
        public async Task Upload(string usrName, IBrowserFile file)
        {
            long fileSize = file.Size;
            List<FileSlim> files = await GetdirsAndFiles(usrName);
            string path = String.Empty;
            if (File.Exists(Path.Combine(_baseDir, usrName, file.Name)))
            {
                string name = String.Concat(file.Name, "(", GetSameNameFilesCount(path), ")");
                path = Path.Combine(_baseDir, usrName, name ).ToString();
            }
            else
            {
                path = Path.Combine(_baseDir, usrName,file.Name).ToString();
            }
            await using FileStream fs = new(path, FileMode.CreateNew,FileAccess.ReadWrite);
            await file.OpenReadStream(fileSize).CopyToAsync(fs);
        }
        /// <summary>
        /// Löscht eine Datei
        /// </summary>
        /// <param name="usrName">Benutzername</param>
        /// <param name="fileName">Dateiname</param>
        /// <returns></returns>
        public async Task Delete(string usrName, string fileName)
        {
            string path = Path.Combine(_baseDir, usrName, fileName).ToString();
            await Task.Run(() => File.Delete(path));
        }
        /// <summary>
        /// Wandelt ein Bild in ein Base64 String um
        /// </summary>
        /// <param name="input">Hochgeladenes Profilbild</param>
        /// <returns></returns>
        public async Task<string> PicToBase64Async(IBrowserFile input)
        {
                byte[] fileBytes;
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    try
                    {
                        
                        await input.OpenReadStream().CopyToAsync(memoryStream);
                        fileBytes = memoryStream.ToArray();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
                string base64String = Convert.ToBase64String(fileBytes);
                return base64String;
        }

        #endregion
    }

}

