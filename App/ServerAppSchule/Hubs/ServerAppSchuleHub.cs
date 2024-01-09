using Microsoft.AspNetCore.SignalR;
using ServerAppSchule.Interfaces;
using ServerAppSchule.Models;

namespace ServerAppSchule.Hubs
{
    public class ServerAppSchuleHub : Hub
    {
        IPostService _postService;
        public ServerAppSchuleHub(IPostService postService)
        {
            _postService = postService;
        }
        public async Task UpdatePosts(Post postdata)
        {
            Post postres = await _postService.CreatePost(postdata);
            await Clients.All.SendAsync("PostCreated",postres);
        }


    }
}
