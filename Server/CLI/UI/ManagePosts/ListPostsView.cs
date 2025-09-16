using System;
using System.Threading.Tasks;
using Entities;
using RepositoryContracts;

namespace CLI.UI.ManagePosts
{
    public class ListPostsView
    {
        private readonly IPostRepository postRepository;

        public ListPostsView(IPostRepository postRepository)
        {
            this.postRepository = postRepository;
        }

        public async Task RunAsync()
        {
            Console.WriteLine("--- Posts Overview ---");
            foreach (Post p in postRepository.GetManyAsync())
            {
                Console.WriteLine("[" + p.Id + "] " + p.Title);
            }
            await Task.CompletedTask;
        }
    }
}