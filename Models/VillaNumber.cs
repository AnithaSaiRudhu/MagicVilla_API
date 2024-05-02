using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagicVilla_VillaAPI.Models
{
    public class VillaNumber
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int villaNo { get; set; }

        [ForeignKey("Villa")]

        public int VillaID { get; set; }

        public Villa? Villa { get; set; }
        public string SpecialDetails { get; set; } = string.Empty;

        public DateTime createdDate { get; set; } = DateTime.Now;

        public DateTime updatedDate { get; set; } = DateTime.MinValue;
    }
}
