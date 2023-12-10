using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Interface;
using MagicVilla_VillaAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_VillaAPI.Repository {

    public class VillaRepository : IVillaRepository {
        private readonly ApplicationDbContext context;

        public VillaRepository(ApplicationDbContext context) {
            this.context = context;
        }

        public bool CheckVillaExistance(string Name) {
            return context.Villas.Where(v => v.Name.ToLower() == Name.ToLower()).Any();
        }

        public bool CreateVilla(Villa villa) {
            context.Villas.Add(villa);
            return Save();
        }

        public bool DeleteVilla(Villa villa) {
            context.Villas.Remove(villa);
            return Save();
        }

        public ICollection<Villa> GetAllVillas() {
            return context.Villas.ToList();
        }

        public Villa GetVillaById(int id) {
            return context.Villas.FirstOrDefault(x => x.Id == id);
        }

        public Villa GetVillaByIdAsNoTracking(int id) {
            return context.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);
        }

        public bool Save() {
            return context.SaveChanges() > 0 ? true : false;
        }

        public bool UpdateVilla(Villa villa) {
            context.Villas.Update(villa);
            return Save();
        }
    }
}
