using System;
using System.Threading.Tasks;
using RepositoryContracts;

namespace CLI.UI.ManageUsers
{
    public class ManageUsersView
    {
        private readonly IUserRepository userRepository;

        public ManageUsersView(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task RunAsync()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== Manage Users ===");
                Console.WriteLine("1) Create user");
                Console.WriteLine("2) List users");
                Console.WriteLine("3) Delete user");
                Console.WriteLine("0) Back");
                Console.Write("Choose: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreateUserView createUserView = new CreateUserView(userRepository);
                        await createUserView.RunAsync();
                        break;
                    case "2":
                        ListUsersView listUsersView = new ListUsersView(userRepository);
                        await listUsersView.RunAsync();
                        break;
                    case "3":
                        DeleteUserView deleteUserView = new DeleteUserView(userRepository);
                        await deleteUserView.RunAsync();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Unknown choice.");
                        break;
                }

                Console.WriteLine();
            }
        }
    }
}