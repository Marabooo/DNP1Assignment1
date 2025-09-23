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
    public class UserFileRepository : IUserRepository
    {
        private const string FilePath = "users.json";

        public UserFileRepository()
        {
            SeedDataAsync().GetAwaiter();
        }

        public async Task<User> AddAsync(User user)
        {
            var users = await GetUsersAsync();
            user.Id = users.Count != 0 ? users.Max(u => u.Id) + 1 : 1;
            users.Add(user);
            await SaveUsersAsync(users);
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var users = await GetUsersAsync();
            User? toRemove = users.SingleOrDefault(u => u.Id == id)
                ?? throw new InvalidOperationException($"User with ID '{id}' not found");
            users.Remove(toRemove);
            await SaveUsersAsync(users);
        }

        public IQueryable<User> GetManyAsync()
        {
            return GetUsersAsync().Result.AsQueryable();
        }

        public async Task<User> GetSingleAsync(int id)
        {
            var users = await GetUsersAsync();
            User? user = users.SingleOrDefault(u => u.Id == id)
                ?? throw new InvalidOperationException($"User with ID '{id}' not found");
            return user;
        }

        public async Task UpdateAsync(User user)
        {
            var users = await GetUsersAsync();
            User? existing = users.SingleOrDefault(u => u.Id == user.Id)
                ?? throw new InvalidOperationException($"User with ID '{user.Id}' not found");
            users.Remove(existing);
            users.Add(user);
            await SaveUsersAsync(users);
        }

        private static async Task<List<User>> GetUsersAsync()
        {
            if (!File.Exists(FilePath))
                await File.WriteAllTextAsync(FilePath, "[]");

            string users = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<User>>(users) ?? new List<User>(); 
        }

        private static async Task SaveUsersAsync(List<User> users)
        {
            await File.WriteAllTextAsync(FilePath, JsonSerializer.Serialize(users));
        }

        private async Task SeedDataAsync()
        {
            if ((await GetUsersAsync()).Count != 0) return;

            var seed = new List<User>
            {
                new User { Id = 1, Username = "alice", Password = "secret" },
                new User { Id = 2, Username = "bob",   Password = "hunter2" }
            };

            await SaveUsersAsync(seed);
        }
    }
}
