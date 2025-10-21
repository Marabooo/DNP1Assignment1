using FileRepositories;
using RepositoryContracts;
using Microsoft.AspNetCore.Builder;
using Scalar.AspNetCore;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IPostRepository, PostFileRepository>();
builder.Services.AddScoped<ICommentRepository, CommentFileRepository>();
builder.Services.AddScoped<IUserRepository, UserFileRepository>();

var app = builder.Build();

app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();                       // serves the OpenAPI JSON

    // Scalar UI (official UI for .NET 9/10)
    app.MapScalarApiReference(options =>
    {
        options.Title = "Assignment 4 API v1";
        // options.Theme = ScalarTheme.Moon; // optional
    });
}
        
app.UseHttpsRedirection();

app.Run();

