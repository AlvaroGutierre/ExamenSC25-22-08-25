using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExamenSC25_AlvaroGutierrez.Models;

namespace ExamenSC25_AlvaroGutierrez.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PeliculasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PeliculasController(AppDbContext context)
        {
            _context = context;
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
            return new {
                items = peliculas,
                totalCount = total,
                totalPages = totalPages,
                page = page,
                pageSize = pageSize
            };
        }

        // GET: api/peliculas/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Pelicula>> GetPelicula(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
                return NotFound();
            return pelicula;
        }

        // POST: api/peliculas
        [HttpPost]
        public async Task<ActionResult<Pelicula>> PostPelicula(Pelicula pelicula)
        {
            _context.Peliculas.Add(pelicula);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPeliculas), new { id = pelicula.Id }, pelicula);
        }

        // PUT: api/peliculas/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPelicula(int id, Pelicula pelicula)
        {
            if (id != pelicula.Id)
                return BadRequest();
            _context.Entry(pelicula).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/peliculas/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePelicula(int id)
        {
            var pelicula = await _context.Peliculas.FindAsync(id);
            if (pelicula == null)
                return NotFound();
            _context.Peliculas.Remove(pelicula);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
