using System;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManageUsers
{
    public class CreateUserView
    {
        private readonly IUserRepository userRepository;

        public CreateUserView(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("--- Create User ---");
            Console.Write("Username: ");
            string username = ReadNonEmpty();
            Console.Write("Password: ");
            string password = ReadNonEmpty();

            User user = new User();
            user.Username = username;
            user.Password = password;

            User created = await userRepository.AddAsync(user);
            Console.WriteLine("User created with Id: " + created.Id);
        }

        private static string ReadNonEmpty()
        {
            while (true)
            {
                string? s = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(s)) return s.Trim();
                Console.Write("Please enter a value: ");
            }
        }
    }
}