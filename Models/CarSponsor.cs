using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Proyecto_FInal_Grupo_1.Models
{
    public class CarSponsor
    {
        public Guid Id { get; set; }

        [Required]
        public string Location { get; set; } 
        public Guid TeamCarId { get; set; }
        public Guid SponsorId { get; set; }
        public TeamCar? TeamCar { get; set; }
        public Sponsor? Sponsor { get; set; }
    }
}