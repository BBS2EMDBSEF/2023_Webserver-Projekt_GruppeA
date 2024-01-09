using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using ServerAppSchule.Models;

namespace ServerAppSchule.Components
{
    partial class CreatePostDialog
    {
        [Parameter]
        public string Uid { get; set; }
        [CascadingParameter]
        MudDialogInstance _mudDialog { get; set; }
        [Inject]
        NavigationManager _navigationManager { get; set; }
        HubConnection? _hubConnection;

        Post _post = new Post();
        protected override void OnInitialized()
        {
            _post = new Post();
            _post.CreatedBy = Uid;
            _hubConnection = new HubConnectionBuilder()
             .WithUrl(_navigationManager.ToAbsoluteUri("/serverappschulehub"))
             .Build();
            _hubConnection.StartAsync();
            base.OnInitialized();
        }

        /// <summary>
        /// Bricht den Dialog ab
        /// </summary>
        private void Cancel() => _mudDialog.Cancel();

        /// <summary>
        /// erstellt einen neuen Post
        /// </summary>
        /// <returns></returns>
        private async Task SubmitAsync()
        {
            await _hubConnection.InvokeAsync("UpdatePosts", _post);
            Cancel();
        }
    }
}
