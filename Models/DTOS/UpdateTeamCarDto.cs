namespace Proyecto_FInal_Grupo_1.Models.DTOS
{
    public record UpdateTeamCarDto
    {
        public string Model { get; set; }
        public string TeamName { get; set; }
        public string Engine { get; set; }
        public int Year { get; set; }
    }
}