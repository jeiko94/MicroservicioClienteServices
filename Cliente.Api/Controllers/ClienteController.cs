using Microsoft.AspNetCore.Mvc;
using Cliente.Api.DTOs;
using Clientes.Aplicacion.Servicios;

namespace Clientes.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly ClienteServicio _clienteServicio;

        public ClientesController(ClienteServicio clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarCliente([FromBody] RegistrarClienteDto dto)
        {
            try
            {
                int idCliente = await _clienteServicio.RegistrarClienteAsync(dto.Nombre, dto.Email, dto.Password);
                return Ok($"Cliente registrado con Id = {idCliente}");
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPost("autenticar")]
        public async Task<IActionResult> Autenticar([FromBody] AutenticarDto dto)
        {
            var cliente = await _clienteServicio.AutenticarClienteAsync(dto.Email, dto.Password);
            if (cliente == null)
                return Unauthorized("Credenciales inválidas.");

            // Podrías devolver un token JWT o solo un mensaje, según tu diseño
            return Ok("Autenticado correctamente (falta JWT si deseas).");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerCliente(int id)
        {
            var cliente = await _clienteServicio.ObtenerClienteAsync(id);
            if (cliente == null)
                return NotFound("Cliente no encontrado.");

            // Podríamos mapear a un ClienteDto
            return Ok(new ClienteDto
            {
                Id = cliente.Id,
                Nombre = cliente.Nombre,
                Email = cliente.Email,
                FechaRegistro = cliente.FechaRegistro,
                Activo = cliente.Activo
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCliente(int id, [FromBody] ActualizarClienteDto dto)
        {
            try
            {
                await _clienteServicio.ActualizarClienteAsync(id, dto.Nombre, dto.Email);
                return Ok("Cliente actualizado correctamente.");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCliente(int id)
        {
            await _clienteServicio.EliminarClienteAsync(id);
            return Ok("Cliente eliminado.");
        }
    }
}
