using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace FileRepositories
{
    public class PostFileRepository : IPostRepository
    {
        private const string FilePath = "posts.json";

        public PostFileRepository()
        {
            SeedDataAsync().GetAwaiter();
        }

        public async Task<Post> AddAsync(Post post)
        {
            var posts = await GetPostsAsync();
            post.Id = posts.Count != 0 ? posts.Max(p => p.Id) + 1 : 1;
            posts.Add(post);
            await SavePostsAsync(posts);
            return post;
        }

        public async Task DeleteAsync(int id)
        {
            var posts = await GetPostsAsync();
            Post? toRemove = posts.SingleOrDefault(p => p.Id == id)
                ?? throw new InvalidOperationException($"Post with ID '{id}' not found");
            posts.Remove(toRemove);
            await SavePostsAsync(posts);
        }

        public IQueryable<Post> GetManyAsync()
        {
            return GetPostsAsync().Result.AsQueryable();
        }

        public async Task<Post> GetSingleAsync(int id)
        {
            var posts = await GetPostsAsync();
            Post? post = posts.SingleOrDefault(p => p.Id == id)
                ?? throw new InvalidOperationException($"Post with ID '{id}' not found");
            return post;
        }

        public async Task UpdateAsync(Post post)
        {
            var posts = await GetPostsAsync();
            Post? existing = posts.SingleOrDefault(p => p.Id == post.Id)
                ?? throw new InvalidOperationException($"Post with ID '{post.Id}' not found");
            posts.Remove(existing);
            posts.Add(post);
            await SavePostsAsync(posts);
        }

        private static async Task<List<Post>> GetPostsAsync()
        {
            if (!File.Exists(FilePath))
                await File.WriteAllTextAsync(FilePath, "[]");

            string posts = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<Post>>(posts) ?? new List<Post>(); 
        }
        private static async Task SavePostsAsync(List<Post> posts)
        {
            await File.WriteAllTextAsync(FilePath, JsonSerializer.Serialize(posts));
        }
        private async Task SeedDataAsync()
        {
            if ((await GetPostsAsync()).Count != 0) return;

            var seed = new List<Post>
            {
                new Post { Id = 1, Title = "Welcome", Body = "Seed post 1", UserId = 1 },
                new Post { Id = 2, Title = "Second", Body = "Seed post 2", UserId = 2 }
            };

            await SavePostsAsync(seed);
        }
    }
}
