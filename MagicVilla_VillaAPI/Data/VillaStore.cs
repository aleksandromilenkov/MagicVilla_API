using MagicVilla_VillaAPI.DTO;

namespace MagicVilla_VillaAPI.Data {
    public static class VillaStore {
        public static List<VillaDTO> villaList = new List<VillaDTO>() {
               new VillaDTO{Id=1, Name="Beach Villa", Occupancy=3, Sqft=300},
               new VillaDTO{Id=2, Name="Sunset Villa", Occupancy=2, Sqft=100}
           };
    }
}
