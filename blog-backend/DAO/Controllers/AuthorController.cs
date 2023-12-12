using blog_backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;

public class AuthorController : GlobalController
{
    private readonly AuthorService _authorService;

    public AuthorController(AuthorService authorService)
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