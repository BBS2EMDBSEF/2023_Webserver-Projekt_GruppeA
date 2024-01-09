using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServerAppSchule.Data;
using ServerAppSchule.Factories;
using ServerAppSchule.Interfaces;
using ServerAppSchule.Models;

namespace ServerAppSchule.Services
{
    public class PostService: IPostService
    {
        private readonly ApplicationDbContextFactory  _contextFactory;
        public PostService(ApplicationDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public Task<Post> CreatePost(Post post)
        {
            using var context = _contextFactory.CreateDbContext();
            post.CreatedAt = DateTime.Now;
            context.Posts.Add(post);
            context.SaveChanges();
            return Task.FromResult(post);

            
        }

        public async Task<List<Post>> GetUserPosts(string uid)
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Posts
                    .Include(p => p.Comments)
                    .Include(p => p.Pictures)
                    .Include(p => p.Likes)
                    .Where(p => p.CreatedBy == uid && !p.IsDeleted)
                    .OrderByDescending(p => p.CreatedAt)
                    .AsNoTracking()
                    .ToListAsync();
        }   
        public async Task<List<Post>> GetAllPosts()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.Posts
                    .Include(p => p.Comments)
                    .Include(p => p.Pictures)
                    .Include(p => p.Likes)
                    .Where(p => !p.IsDeleted)
                    .OrderByDescending(p => p.CreatedAt)
                    .AsNoTracking()
                    .ToListAsync();
        }
    }
}
