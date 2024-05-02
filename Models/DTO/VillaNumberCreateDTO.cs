using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_VillaAPI.Models
{
    public class VillaNumberCreateDTO
    {

        [Required]

        public int villaNo { get; set; }


        [Required]
        public int VillaID { get; set; }

        public string SpecialDetails { get; set; } = string.Empty;

        public DateTime createdDate { get; set; } = DateTime.Now;

       
    }
}
