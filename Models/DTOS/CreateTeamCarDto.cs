using System.ComponentModel.DataAnnotations;

namespace Proyecto_FInal_Grupo_1.Models.DTOS
{
    public record CreateTeamCarDto
    {
        [Required]
        public string Model { get; set; }
        [Required]
        public string TeamName { get; set; }
        public string Engine { get; set; }
        public int Year { get; set; }
    }
}