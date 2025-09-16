using System;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts
{
    public class CreatePostView
    {
        private readonly IPostRepository postRepository;
        private readonly IUserRepository userRepository;

        public CreatePostView(IPostRepository postRepository, IUserRepository userRepository)
        {
            this.postRepository = postRepository;
            this.userRepository = userRepository;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("--- Create Post ---");
            Console.Write("Title: ");
            string title = ReadNonEmpty();
            Console.Write("Body: ");
            string body = ReadNonEmpty();
            Console.Write("User Id: ");
            int userId = ReadInt();

            try
            {
                await userRepository.GetSingleAsync(userId);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("No such user Id.");
                return;
            }

            Post post = new Post();
            post.Title = title;
            post.Body = body;
            post.UserId = userId;

            Post created = await postRepository.AddAsync(post);
            Console.WriteLine("Post created with Id: " + created.Id);
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