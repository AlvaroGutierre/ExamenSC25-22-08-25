using Microsoft.EntityFrameworkCore;
using ExamenSC25_AlvaroGutierrez.Models;

namespace ExamenSC25_AlvaroGutierrez.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Pelicula> Peliculas { get; set; }
    }
}
