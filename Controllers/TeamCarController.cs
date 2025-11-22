using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Proyecto_FInal_Grupo_1.Models;
using Proyecto_FInal_Grupo_1.Models.DTOS;
using Proyecto_FInal_Grupo_1.Services;

namespace Proyecto_FInal_Grupo_1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamCarController : ControllerBase
    {
        private readonly ITeamCarService _service;
        private readonly ILogger<TeamCarController> _logger;

        public TeamCarController(ITeamCarService service, ILogger<TeamCarController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Obteniendo todos los autos");
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetOne(Guid id)
        {
            var car = await _service.GetOne(id);
            if (car == null)
            {
                _logger.LogWarning($"Auto con ID {id} no encontrado");
                return NotFound();
            }
            return Ok(car);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateTeamCarDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var car = await _service.Create(dto);
                _logger.LogInformation($"Auto creado: {car.TeamName} - {car.Model}");
                return CreatedAtAction(nameof(GetOne), new { id = car.Id }, car); 
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creando el auto"); 
                return StatusCode(500, "Error interno del servidor"); 
            }
        }

        [HttpPut("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTeamCarDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                var car = await _service.Update(dto, id);
                return Ok(car);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error actualizando auto {id}: {ex.Message}");
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.Delete(id);
            _logger.LogInformation($"Auto {id} eliminado");
            return NoContent();
        }
    }
}