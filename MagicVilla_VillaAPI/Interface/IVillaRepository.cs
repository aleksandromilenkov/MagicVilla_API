using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Interface {
    public interface IVillaRepository {
        ICollection<Villa> GetAllVillas();
        Villa GetVillaById(int id);
        Villa GetVillaByIdAsNoTracking(int id);
        bool CheckVillaExistance(string Name);
        bool CreateVilla(Villa villa);
        bool UpdateVilla(Villa villa);
        bool DeleteVilla(Villa villa);
        bool Save();

    }
}
