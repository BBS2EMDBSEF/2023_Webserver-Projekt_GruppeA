using ServerAppSchule.Models;

namespace ServerAppSchule.Interfaces
{
    public interface IPostService
    {
        Task<List<Post>> GetUserPosts(string uid);
        Task<Post> CreatePost(Post post);
        Task<List<Post>> GetAllPosts();
    }
}
