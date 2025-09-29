// See https://aka.ms/new-console-template for more information

using System.Runtime.InteropServices.ComTypes;
using CLI.UI;
using RepositoryContracts;
using FileRepositories;

Console.WriteLine("Starting CLI app...");
IUserRepository userRepository = new UserFileRepository();
IPostRepository postRepository = new PostFileRepository();
ICommentRepository commentRepository = new CommentFileRepository();

CliApp cliApp = new CliApp(userRepository, commentRepository, postRepository);
await cliApp.StartAsync();