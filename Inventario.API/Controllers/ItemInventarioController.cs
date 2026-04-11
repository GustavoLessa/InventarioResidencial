using Inventario.Application.DTOs;
using Inventario.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventario.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemInventarioController : ControllerBase
{
    private readonly IItemAppService _itemService;

    public ItemInventarioController(IItemAppService itemService)
    {
        _itemService = itemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemInventarioDTO>>> GetAll()
    {
        var itens = await _itemService.GetAllAsync();
        return Ok(itens);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ItemInventarioDTO>> GetById(Guid id)
    {
        var item = await _itemService.GetByIdAsync(id);
        if (item == null) return NotFound();

        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<ItemInventarioDTO>> Create([FromBody] CreateItemInventarioDTO item)
    {
        var createdItemId = await _itemService.AddAsync(item);
        if (createdItemId is null) return StatusCode(500, "Erro ao salvar o item.");

        var createdItem = await _itemService.GetByIdAsync(createdItemId.Value);
        if (createdItem == null) return StatusCode(500, "Item criado, mas nao foi possivel recupera-lo.");

        return CreatedAtAction(nameof(GetById), new { id = createdItem.Id }, createdItem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateItemInventarioDTO dto)
    {
        var sucesso = await _itemService.UpdateAsync(id, dto);
        if (!sucesso) return NotFound("Item não encontrado ou erro na atualização.");

        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var sucesso = await _itemService.DeleteAsync(id);
        if (!sucesso) return NotFound("Item não encontrado.");

        return Ok(new { message = "Item removido com sucesso!" });
    }
}
