using Microsoft.AspNetCore.Mvc;
using Business.Api.Application.DTOs;
using Business.Api.Application.Services;

namespace Business.Api.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(IUsuarioService usuarioService, ILogger<UsuariosController> logger)
    {
        _usuarioService = usuarioService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Agregar([FromBody] UsuarioRequestDto request)
    {
        try
        {
            var id = await _usuarioService.AgregarUsuarioAsync(request);
            return CreatedAtAction(nameof(ConsultarPorId), new { id }, id);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar usuario");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Modificar(int id, [FromBody] UsuarioRequestDto request)
    {
        try
        {
            var resultado = await _usuarioService.ModificarUsuarioAsync(id, request);
            if (!resultado)
            {
                return NotFound(new { message = $"Usuario con ID {id} no modificado" });
            }
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al modificar usuario {Id}", id);
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Consultar()
    {
        try
        {
            var usuarios = await _usuarioService.ConsultarUsuariosAsync();
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al consultar usuarios");
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ConsultarPorId(int id)
    {
        try
        {
            var usuario = await _usuarioService.ConsultarUsuarioPorIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new { message = $"Usuario con ID {id} no encontrado" });
            }
            return Ok(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al consultar usuario {Id}", id);
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var resultado = await _usuarioService.EliminarUsuarioAsync(id);
            if (!resultado)
            {
                return NotFound(new { message = $"Usuario con ID {id} no encontrado" });
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar usuario {Id}", id);
            return StatusCode(500, new { message = "Error interno del servidor" });
        }
    }
}
