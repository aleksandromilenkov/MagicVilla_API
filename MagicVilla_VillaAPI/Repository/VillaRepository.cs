using MagicVilla_VillaAPI.Interface;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository {
    public class VillaRepository : IVillaRepository {
        public VillaRepository() { }
        public ICollection<Villa> GetAllVillas() {
            return new List<Villa>() {
               new Villa{Id=1, Name="Beach Villa"},
               new Villa{Id=2, Name="Sunset Villa"}
           };
        }

        public Villa GetVillaById(int id) {
            return new Villa { Id = id, Name = "Beach Villa" };
        }
    }
}
