using ExamenSC25_AlvaroGutierrez.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Configuración de DbContext con SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=peliculas.db"));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Seed de la base de datos con 50 películas si está vacía
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    if (!db.Peliculas.Any())
    {
        var generos = new[] { "Acción", "Comedia", "Drama", "Terror", "Ciencia Ficción", "Aventura", "Animación" };
        var directores = new[] { "Spielberg", "Nolan", "Tarantino", "Scorsese", "Cameron", "Burton", "Fincher" };
        var rnd = new Random();
        var peliculas = Enumerable.Range(1, 50).Select(i => new Pelicula
        {
            Titulo = $"Pelicula {i}",
            Director = directores[rnd.Next(directores.Length)],
            FechaEstreno = rnd.Next(1980, 2024),
            Genero = generos[rnd.Next(generos.Length)],
            Duracion = rnd.Next(80, 180)
        }).ToList();
        db.Peliculas.AddRange(peliculas);
        db.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("FrontendPolicy");

app.MapControllers();

app.Run();
