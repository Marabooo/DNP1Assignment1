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
    public class CommentFileRepository : ICommentRepository
    {
        private const string FilePath = "comments.json";

        public CommentFileRepository()
        {
            SeedDataAsync().GetAwaiter();
        }

        public async Task<Comment> AddAsync(Comment comment)
        {
            var comments = await GetCommentsAsync();
            comment.Id = comments.Count != 0 ? comments.Max(c => c.Id) + 1 : 1;
            comments.Add(comment);
            await SaveCommentsAsync(comments);
            return comment;
        }

        public async Task DeleteAsync(int id)
        {
            var comments = await GetCommentsAsync();
            Comment? commentToRemove = comments.SingleOrDefault(c => c.Id == id)
                ?? throw new InvalidOperationException($"Comment with ID '{id}' not found");
            comments.Remove(commentToRemove);
            await SaveCommentsAsync(comments);
        }

        public IQueryable<Comment> GetManyAsync()
        {
            return GetCommentsAsync().Result.AsQueryable();
        }

        public async Task<Comment> GetSingleAsync(int id)
        {
            var comments = await GetCommentsAsync();
            Comment? comment = comments.SingleOrDefault(c => c.Id == id)
                ?? throw new InvalidOperationException($"Comment with ID '{id}' not found");
            return comment;
        }

        public async Task UpdateAsync(Comment comment)
        {
            var comments = await GetCommentsAsync();
            Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id)
                ?? throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found");
            comments.Remove(existingComment);
            comments.Add(comment);
            await SaveCommentsAsync(comments);
        }

        // ----- helpers (exactly like in the photos) -----

        private static async Task<List<Comment>> GetCommentsAsync()
        {
            if (!File.Exists(FilePath))
                await File.WriteAllTextAsync(FilePath, "[]");

            string comments = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<Comment>>(comments) ?? new List<Comment>(); 
        }

        private static async Task SaveCommentsAsync(List<Comment> comments)
        {
            await File.WriteAllTextAsync(FilePath, JsonSerializer.Serialize(comments));
        }

        private async Task SeedDataAsync()
        {
            if ((await GetCommentsAsync()).Count != 0) return; 

            var comment1 = new Comment
            {
                Id = 1,
                Body = "comment1",
                PostId = 1,
                UserId = 1
            };

            var comment2 = new Comment
            {
                Id = 2,
                Body = "comment2",
                PostId = 2,
                UserId = 2
            };

            await SaveCommentsAsync(new List<Comment> { comment1, comment2 });
        }
    }
}
