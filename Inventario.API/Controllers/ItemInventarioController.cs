using Microsoft.AspNetCore.Mvc;
using Inventario.Application.Interfaces; // Namespace da tua camada de Application
using Inventario.Domain.Entities;
using Inventario.Application.DTOs;
using Microsoft.AspNetCore.Authorization;      // Namespace da tua camada de Domain

namespace Inventario.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ItemInventarioController : ControllerBase
    {
        private readonly IItemAppService _itemService;        

        // Injeção de Dependência da Interface de Aplicação
        public ItemInventarioController(IItemAppService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemInventarioDTO>> GetById(Guid id)
        {
            var item = await _itemService.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateItemInventarioDTO item)
        {
            if (item == null) return BadRequest();

            var success = await _itemService.AddAsync(item);
            
            if (!success) return StatusCode(500, "Erro ao salvar o item.");

            // Agora retornamos o status 201 (Created) seguindo as boas práticas
            return CreatedAtAction(nameof(GetById), new { id = item }, item); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] CreateItemInventarioDTO dto)
        {
            var sucesso = await _itemService.UpdateAsync(id, dto);
            if (!sucesso) return NotFound("Item não encontrado ou erro na atualização.");

            return NoContent(); // 204 No Content é o padrão para updates bem-sucedidos
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var sucesso = await _itemService.DeleteAsync(id);
            if (!sucesso) return NotFound("Item não encontrado.");

            return Ok(new { message = "Item removido com sucesso!" });
        }
    }
}