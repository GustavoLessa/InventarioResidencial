using Inventario.Application.DTOs;
using Inventario.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemInventarioController : ControllerBase
{
    private const string UserIdHeaderName = "X-User-Id";
    private readonly IItemAppService _itemService;

    public ItemInventarioController(IItemAppService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemInventarioDTO>>> GetAll()
    {
        var userId = GetUserId();
        if (userId == null) return BadRequest($"Informe o header {UserIdHeaderName}.");

        var itens = await _itemService.GetAllAsync(userId);
        return Ok(itens);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ItemInventarioDTO>> GetById(Guid id)
    {
        var userId = GetUserId();
        if (userId == null) return BadRequest($"Informe o header {UserIdHeaderName}.");

        var item = await _itemService.GetByIdAsync(id, userId);
        if (item == null) return NotFound();

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ItemInventarioDTO>> Create([FromBody] CreateItemInventarioDTO item)
    {
        var userId = GetUserId();
        if (userId == null) return BadRequest($"Informe o header {UserIdHeaderName}.");

        var createdItemId = await _itemService.AddAsync(item, userId);
        if (createdItemId is null) return StatusCode(500, "Erro ao salvar o item.");

        var createdItem = await _itemService.GetByIdAsync(createdItemId.Value, userId);
        if (createdItem == null) return StatusCode(500, "Item criado, mas nao foi possivel recupera-lo.");

        return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, createdItem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateItemInventarioDTO dto)
    {
        var userId = GetUserId();
        if (userId == null) return BadRequest($"Informe o header {UserIdHeaderName}.");

        var sucesso = await _itemService.UpdateAsync(id, dto, userId);
        if (!sucesso) return NotFound("Item nao encontrado ou erro na atualizacao.");

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetUserId();
        if (userId == null) return BadRequest($"Informe o header {UserIdHeaderName}.");

        var sucesso = await _itemService.DeleteAsync(id, userId);
        if (!sucesso) return NotFound("Item nao encontrado.");

        return Ok(new { message = "Item removido com sucesso!" });
    }

    private string? GetUserId()
    {
        if (!Request.Headers.TryGetValue(UserIdHeaderName, out var userIdValues))
        {
            return null;
        }

        var userId = userIdValues.ToString().Trim();
        return string.IsNullOrWhiteSpace(userId) ? null : userId;
    }
}
