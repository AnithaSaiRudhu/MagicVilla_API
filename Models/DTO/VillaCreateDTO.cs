﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.DTO
{
    public class VillaCreateDTO
    {
       
        [Required]        
       public string? Name { get; set; }


        public string? Details { get; set; }

        [Required]
        public double Rate { get; set; }

        public int SqFt { get; set; }

        public int Occupancy { get; set; }

        public string? ImageUrl { get; set; } 

        public string? Amenity { get; set; }

    }
}
