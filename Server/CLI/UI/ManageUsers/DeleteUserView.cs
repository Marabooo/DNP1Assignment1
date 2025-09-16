using System;
using System.Threading.Tasks;
using RepositoryContracts;

namespace CLI.UI.ManageUsers
{
    public class DeleteUserView
    {
        private readonly IUserRepository userRepository;

        public DeleteUserView(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("--- Delete User ---");
            Console.Write("User Id: ");
            int id = ReadInt();

            try
            {
                await userRepository.DeleteAsync(id);
                Console.WriteLine("User with Id " + id + " deleted.");
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("User with Id " + id + " not found.");
            }
        }

        private static int ReadInt()
        {
            while (true)
            {
                string? s = Console.ReadLine();
                int value;
                if (int.TryParse(s, out value)) return value;
                Console.Write("Please enter a valid number: ");
            }
        }
    }
}