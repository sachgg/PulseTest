using Application.Clients;
using Domain.Clients;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController : Controller
{
    private readonly IClientService _clientService;
    public ClientController(IClientService clientService)
    {
        _clientService = clientService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _clientService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id) 
    {
        var client = await _clientService.Get(id);

        return client == null ? NotFound() : Ok(client);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateClient client)
    {
        var id = await _clientService.Create(client);

        return CreatedAtAction(nameof(Get), new { id }, client);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CreateClient client)
    {
        var success = await _clientService.Update(id, client);
        
        return success ? NoContent() : NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id) 
    {
        var success = await _clientService.Delete(id);
        return success ? NoContent() : NotFound();
    }
}