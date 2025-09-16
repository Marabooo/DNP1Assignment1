using System;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers
{
    public class ListUsersView
    {
        private readonly IUserRepository userRepository;

        public ListUsersView(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("--- Users ---");
            foreach (User user in userRepository.GetManyAsync())
            {
                Console.WriteLine("[" + user.Id + "] " + user.Username);
            }
            await Task.CompletedTask;
        }
    }
}