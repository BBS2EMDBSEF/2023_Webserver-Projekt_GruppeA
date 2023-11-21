using BlazorDownloadFile;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Hosting;
using Microsoft.JSInterop;
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
        protected override async Task OnInitializedAsync()
        {

            var currentauth = await _authenticationState;
            if (!currentauth.User.Identity.IsAuthenticated)
            {
                if(!(currentauth.User.IsInRole("FTPUser") || currentauth.User.IsInRole("Admin")))
                {
                    _navManager.NavigateTo("/", true);
                }
                else
                {
                    _navManager.NavigateTo("/login", true);
                }
            }
            _usr = currentauth.User.Identity.Name;
            _loading = true;
            _files = _fileService.GetdirsAndFiles(_usr);
            _loading = false;
        }
        async Task DownloadFile(string filename)
        {
            string filePath = _fileService.DownloadPath(_usr, filename).ToString();
            byte[] fileBytes = await File.ReadAllBytesAsync(filePath);
            var task = await _blazorDownloadFileService.DownloadFile(filename, fileBytes.ToList(), CancellationToken.None, "application/octet-stream");
        }
        async Task DownloadZip(string filename)
        {
            byte[] fileBytes = _fileService.ZipDirectoryAsync(filename, _usr).Result;
            var Task = await _blazorDownloadFileService.DownloadFile(filename + ".zip", fileBytes.ToList(), CancellationToken.None, "application/octet-stream");
        }
        void UploadFiles(IReadOnlyList<IBrowserFile> files)
        {
        }
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
