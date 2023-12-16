using blog_backend.DAO.IService;
using blog_backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;

public class AuthorController : GlobalController
{
    private readonly IAuthorService _authorService;

    public AuthorController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet("author/list")]
    public async Task<IActionResult> GetAuthorList()
    {
        var authorList = await _authorService.GetAuthorList();
        return Ok(authorList);
    }
}