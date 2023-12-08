using MagicVilla_VillaAPI.DTO;
using MagicVilla_VillaAPI.Interface;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers {
    [Route("api/Villa")]
    [ApiController]
    public class VillaController : Controller {
        private readonly IVillaRepository villaRepository;

        public VillaController(IVillaRepository villaRepository) {
            this.villaRepository = villaRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VillaDTO>))]
        public IActionResult GetVillas() {
            var list = villaRepository.GetAllVillas();
            return Ok(list);
        }

        [HttpGet("id")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetVillaById(int id) {
            if (id <= 0) {
                return BadRequest();
            }
            if (id == null) {
                return NotFound();
            }
            var villa = villaRepository.GetVillaById(id);
            var villaDTO = new VillaDTO {
                Id = villa.Id,
                Name = villa.Name
            };
            return Ok(villa);
        }
    }
}
