using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.DTO
{
    public class VillaUpdateDTO
    {
        [Required]
       public int Id { get; set; }
        [Required]
        
       public string? Name { get; set; }


        public string? Details { get; set; }

        [Required]
        public double Rate { get; set; }
        [Required]
        public int SqFt { get; set; }
        [Required]
        public int Occupancy { get; set; }
        [Required]
        public string? ImageUrl { get; set; } 

        public string? Amenity { get; set; }

    }
}
