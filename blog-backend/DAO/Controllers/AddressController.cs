using blog_backend.DAO.IService;
using Microsoft.AspNetCore.Mvc;

namespace blog_backend.DAO.Controllers;

public class AddressController : GlobalController
{
    private readonly IGarService _garService;

    public AddressController(IGarService garService)
    {
        _garService = garService;
    }
    
    [HttpGet]
    [Route("address/hierarchy")]
    public async Task<IActionResult> GetHierarchy([FromQuery] Guid objectGuid)
    {
        var result = await _garService.GetAllHierarchy(objectGuid);
        return Ok(result);
    }

    [HttpGet]
    [Route("address/search")]
    public async Task<IActionResult> SearchAddress([FromQuery] string query = "", [FromQuery] int parentObjectId = 0)
    {
        var result = await _garService.SearchAddressAsync(parentObjectId, query);
        return Ok(result);
    }
    
}