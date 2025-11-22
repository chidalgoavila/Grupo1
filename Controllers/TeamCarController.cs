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

        public TeamCarController(ITeamCarService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAll());
        }

        [HttpGet("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> GetOne(Guid id)
        {
            var car = await _service.GetOne(id);
            if (car == null) return NotFound();
            return Ok(car);
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create([FromBody] CreateTeamCarDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var car = await _service.Create(dto);
            return CreatedAtAction(nameof(GetOne), new { id = car.Id }, car);
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
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _service.Delete(id);
            return NoContent();
        }
    }
}