using blog_backend.DAO.Model;
using blog_backend.Service.Repository;

namespace blog_backend.Service;

public class AuthorService
{
    private readonly IPostRepository _postRepository;
    private readonly IAccountRepository _accountRepository;

    public AuthorService(IPostRepository postRepository, IAccountRepository accountRepository)
    {
        _postRepository = postRepository;
        _accountRepository = accountRepository;
    }

    public Task<List<AuthorDTO>> GetAuthorList()
    {
        var authors = _accountRepository.GetAuthors().Result;
        
        var authorList = (from author in authors
            let posts = _postRepository.GetPostsByAuthor(author.Id.ToString()).Result
            where posts.Count > 0
            select new AuthorDTO
            {
                FullName = author.FullName,
                BirthDate = author.DateOfBirth,
                Posts = posts.Count,
                Created = author.CreateTime,
                Gender = author.Gender,
                Likes = posts.Sum(p => p.Likes)
            }).ToList();
        
        return Task.FromResult(authorList);
    }
}