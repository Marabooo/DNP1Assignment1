using System;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts
{
    public class SinglePostView
    {
        private readonly IPostRepository postRepository;
        private readonly ICommentRepository commentRepository;

        public SinglePostView(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            this.postRepository = postRepository;
            this.commentRepository = commentRepository;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("--- View Post ---");
            Console.Write("Post Id: ");
            int postId = ReadInt();

            Post post;
            try
            {
                post = await postRepository.GetSingleAsync(postId);
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("No such post Id.");
                return;
            }

            Console.WriteLine("Title: " + post.Title);
            Console.WriteLine("Body: " + post.Body);
            Console.WriteLine("--- Comments ---");
            bool any = false;

            foreach (Comment c in commentRepository.GetManyAsync())
            {
                if (c.PostId == post.Id)
                {
                    Console.WriteLine("#" + c.Id + " (User " + c.UserId + "): " + c.Body);
                    any = true;
                }
            }

            if (!any)
            {
                Console.WriteLine("(no comments)");
            }

            await Task.CompletedTask;
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