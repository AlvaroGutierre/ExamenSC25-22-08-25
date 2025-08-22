namespace ExamenSC25_AlvaroGutierrez.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Director { get; set; }
        public int FechaEstreno { get; set; }
        public string Genero { get; set; }
        public int Duracion { get; set; } // Duración en minutos
    }
}