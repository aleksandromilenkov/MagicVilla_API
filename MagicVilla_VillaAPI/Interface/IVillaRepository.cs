using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Interface {
    public interface IVillaRepository {
        ICollection<Villa> GetAllVillas();
        Villa GetVillaById(int id);
    }
}
