using System.ComponentModel.DataAnnotations;

namespace ExamenSC25_AlvaroGutierrez.Models
{
    public class PeliculaDto
    {
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(50)]
        public string Director { get; set; }

        [Range(1900, 2100)]
        public int FechaEstreno { get; set; }

        [Required]
        [StringLength(30)]
        public string Genero { get; set; }

        [Range(1, 500)]
        public int Duracion { get; set; }
    }
}
