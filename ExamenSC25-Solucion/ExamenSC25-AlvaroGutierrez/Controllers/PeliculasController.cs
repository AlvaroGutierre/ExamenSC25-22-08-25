using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamenSC25_AlvaroGutierrez.Models;
using Microsoft.Extensions.Logging;

namespace ExamenSC25_AlvaroGutierrez.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeliculasController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PeliculasController> _logger;

        public PeliculasController(AppDbContext context, ILogger<PeliculasController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/peliculas
        [HttpGet]
        public async Task<ActionResult<object>> GetPeliculas(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? genero = null,
            [FromQuery] string? director = null,
            [FromQuery] int? fechaEstreno = null)
        {
            try
            {
                if (page < 1)
                    return BadRequest("El parámetro 'page' debe ser mayor o igual a 1.");
                if (pageSize < 1 || pageSize > 100)
                    return BadRequest("El parámetro 'pageSize' debe estar entre 1 y 100.");
                var query = _context.Peliculas.AsQueryable();
                if (!string.IsNullOrEmpty(genero))
                    query = query.Where(p => p.Genero == genero);
                if (!string.IsNullOrEmpty(director))
                    query = query.Where(p => p.Director == director);
                if (fechaEstreno.HasValue)
                    query = query.Where(p => p.FechaEstreno == fechaEstreno.Value);
                var total = await query.CountAsync();
                var totalPages = (int)Math.Ceiling(total / (double)pageSize);
                var peliculas = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();
                var items = peliculas.Select(p => new PeliculaDto
                {
                    Titulo = p.Titulo,
                    Director = p.Director,
                    FechaEstreno = p.FechaEstreno,
                    Genero = p.Genero,
                    Duracion = p.Duracion
                }).ToList();
                _logger.LogInformation("Consulta paginada de películas realizada. Página: {Page}, Tamaño: {PageSize}", page, pageSize);
                return new {
                    items = items,
                    totalCount = total,
                    totalPages = totalPages,
                    page = page,
                    pageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetPeliculas: {Message}", ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/peliculas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PeliculaDto>> GetPelicula(int id)
        {
            try
            {
                var pelicula = await _context.Peliculas.FindAsync(id);
                if (pelicula == null)
                {
                    _logger.LogWarning("Película no encontrada. Id: {Id}", id);
                    return NotFound();
                }
                var dto = new PeliculaDto
                {
                    Titulo = pelicula.Titulo,
                    Director = pelicula.Director,
                    FechaEstreno = pelicula.FechaEstreno,
                    Genero = pelicula.Genero,
                    Duracion = pelicula.Duracion
                };
                _logger.LogInformation("Película consultada por id. Id: {Id}", id);
                return dto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetPelicula: {Message}", ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // POST: api/peliculas
        [HttpPost]
        public async Task<ActionResult<Pelicula>> PostPelicula([FromBody] PeliculaDto peliculaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var pelicula = new Pelicula
                {
                    Titulo = peliculaDto.Titulo,
                    Director = peliculaDto.Director,
                    FechaEstreno = peliculaDto.FechaEstreno,
                    Genero = peliculaDto.Genero,
                    Duracion = peliculaDto.Duracion
                };
                _context.Peliculas.Add(pelicula);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Película creada. Id: {Id}", pelicula.Id);
                return CreatedAtAction(nameof(GetPeliculas), new { id = pelicula.Id }, pelicula);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PostPelicula: {Message}", ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // PUT: api/peliculas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPelicula(int id, [FromBody] PeliculaDto peliculaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var pelicula = await _context.Peliculas.FindAsync(id);
                if (pelicula == null)
                {
                    _logger.LogWarning("Intento de actualizar película no existente. Id: {Id}", id);
                    return NotFound();
                }
                pelicula.Titulo = peliculaDto.Titulo;
                pelicula.Director = peliculaDto.Director;
                pelicula.FechaEstreno = peliculaDto.FechaEstreno;
                pelicula.Genero = peliculaDto.Genero;
                pelicula.Duracion = peliculaDto.Duracion;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Película actualizada. Id: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PutPelicula: {Message}", ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // DELETE: api/peliculas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePelicula(int id)
        {
            try
            {
                var pelicula = await _context.Peliculas.FindAsync(id);
                if (pelicula == null)
                {
                    _logger.LogWarning("Intento de eliminar película no existente. Id: {Id}", id);
                    return NotFound();
                }
                _context.Peliculas.Remove(pelicula);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Película eliminada. Id: {Id}", id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en DeletePelicula: {Message}", ex.Message);
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
