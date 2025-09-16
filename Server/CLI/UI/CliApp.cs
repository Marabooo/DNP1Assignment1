using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp (IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
{
    public async Task<Post> StartAsync()
    {
        return null;
    }
    

}