using BlazorDownloadFile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.JSInterop;
using MimeKit.Cryptography;
using MudBlazor.Extensions;
using Org.BouncyCastle.Utilities;
using ServerAppSchule.Models;
using ServerAppSchule.Services;
namespace ServerAppSchule.Pages
{
    partial class FileExplorer
    {
        [Inject]
        NavigationManager _navManager { get; set; }
        [Inject]
        IFileService _fileService { get; set; }
        [CascadingParameter]
        Task<AuthenticationState> _authenticationState { get; set; }
        [Inject]
        private IJSRuntime _jsRuntime { get; set; }
        [Inject] 
        IBlazorDownloadFileService _blazorDownloadFileService { get; set; }
        List<FileSlim> _files = new List<FileSlim>();
        IList<IBrowserFile> _filesToUpload = new List<IBrowserFile>();
        bool _loading = false;
        string _usr = string.Empty;
        private int maxAllowedFiles = 15;
        protected override async Task OnInitializedAsync()
        {

            var currentauth = await _authenticationState;
            if (!currentauth.User.Identity.IsAuthenticated)
            {               
               _navManager.NavigateTo("/login", true);
            }
            _usr = currentauth.User.Identity.Name;
            _loading = true;
            _files = await _fileService.GetdirsAndFiles(_usr);
            if(_files == null)
            {
               await _jsRuntime.InvokeVoidAsync("alert", "Keine Dateien vorhanden");
               _navManager.NavigateTo("/", true);
            }
            _loading = false;
        }
        /// <summary>
        /// Lädt eine Datei runter
        /// </summary>
        /// <param name="filename">Name der Datei die Heruntergeaden werden soll</param>
        /// <returns>Datei download</returns>
        async Task DownloadFile(string filename)
        {
            string filePath = _fileService.DownloadPath(_usr, filename).ToString();
            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
            var task = await _blazorDownloadFileService.DownloadFile(filename, fileBytes.ToList(), CancellationToken.None, "application/octet-stream");
        }
        //async Task DownloadZip(string filename)
        //{
        //    byte[] fileBytes = _fileService.ZipDirectoryAsync(filename, _usr).Result;
        //    var Task = await _blazorDownloadFileService.DownloadFile(filename + ".zip", fileBytes.ToList(), CancellationToken.None, "application/octet-stream");
        //}
        /// <summary>
        /// Lädt eine oder mehrere Dateien hoch
        /// </summary>
        /// <param name="files">Dateien die heruntergeladen werden sollen</param>
        /// <returns></returns>
        private async Task UploadFilesAsync(InputFileChangeEventArgs files)
        {
            try
            {
                foreach (var file in files.GetMultipleFiles())
                {
                    await _fileService.Upload(_usr, file);
                }
            }
            catch (Exception ex)
            {
                  await _jsRuntime.InvokeVoidAsync("alert", ex.Message);
            }

           
        }

        /// <summary>
        /// Formartiert die Datei Größe
        /// </summary>
        /// <param name="size">Datei größe als Zahl</param>
        /// <returns>Datei größe als string</returns>
        string getFormatedFileSize(double? size)
        {
            if (size == null)
            {
                return "";
            }
            else
            {
                return _fileService.FileSizeFormater((long)size);
            }
        }

    }
}
