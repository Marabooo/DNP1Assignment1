using System;
using System.Threading.Tasks;
using RepositoryContracts;

namespace CLI.UI.ManagePosts
{
    public class ManagePostsView
    {
        private readonly IPostRepository postRepository;
        private readonly IUserRepository userRepository;
        private readonly ICommentRepository commentRepository;

        public ManagePostsView(IPostRepository postRepository, IUserRepository userRepository, ICommentRepository commentRepository)
        {
            this.postRepository = postRepository;
            this.userRepository = userRepository;
            this.commentRepository = commentRepository;
        }

        public async Task RunAsync()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine("=== Manage Posts ===");
                Console.WriteLine("1) Create post");
                Console.WriteLine("2) List posts overview");
                Console.WriteLine("3) View single post (with comments)");
                Console.WriteLine("0) Back");
                Console.Write("Choose: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        CreatePostView createPostView = new CreatePostView(postRepository, userRepository);
                        await createPostView.RunAsync();
                        break;
                    case "2":
                        ListPostsView listPostsView = new ListPostsView(postRepository);
                        await listPostsView.RunAsync();
                        break;
                    case "3":
                        SinglePostView singlePostView = new SinglePostView(postRepository, commentRepository);
                        await singlePostView.RunAsync();
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
