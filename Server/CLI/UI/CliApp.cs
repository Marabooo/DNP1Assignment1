using System;
using System.Threading.Tasks;
using RepositoryContracts;
using CLI.UI.ManageUsers;
using CLI.UI.ManagePosts;

namespace CLI.UI
{
    public class CliApp
    {
        private readonly IUserRepository userRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IPostRepository postRepository;

        public CliApp(IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
        {
            this.userRepository = userRepository;
            this.commentRepository = commentRepository;
            this.postRepository = postRepository;
        }

        public async Task StartAsync()
        {
            bool running = true;
            while (running)
            {
                Console.WriteLine("=== Main Menu ===");
                Console.WriteLine("1) Manage Users");
                Console.WriteLine("2) Manage Posts");
                Console.WriteLine("0) Exit");
                Console.Write("Choose: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ManageUsersView usersView = new ManageUsersView(userRepository);
                        await usersView.RunAsync();
                        break;
                    case "2":
                        ManagePostsView postsView = new ManagePostsView(postRepository, userRepository, commentRepository);
                        await postsView.RunAsync();
                        break;
                    case "0":
                        running = false;
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