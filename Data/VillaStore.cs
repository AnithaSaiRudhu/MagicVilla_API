using MagicVilla_VillaAPI.Models.DTO;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>()
            {
                new VillaDTO { Id = 1, Name = "ITCGrancholla" },
                new VillaDTO { Id = 2, Name = "Duplex" },
                new VillaDTO { Id = 3, Name = "Beach Villa" }
            };
    }
}
