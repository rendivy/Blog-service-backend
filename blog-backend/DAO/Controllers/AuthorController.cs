using blog_backend.DAO.Model;
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
    public ActionResult<List<AuthorDTO>> GetAuthorList()
    {
        try
        {
            var authorList = _authorService.GetAuthorList().Result;
            if (authorList.Count == 0) return NotFound();
            return Ok(authorList);
        }
        catch (Exception e)
        {
            var error = new ErrorDTO { Message = e.Message, Status = BadRequest().StatusCode.ToString() };
            return BadRequest(error);
        }
    }
}