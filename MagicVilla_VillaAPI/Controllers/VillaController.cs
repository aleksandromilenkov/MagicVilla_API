using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.DTO;
using MagicVilla_VillaAPI.Interface;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers {
    [Route("api/Villa")]
    [ApiController]
    public class VillaController : Controller {
        private readonly IVillaRepository villaRepository;
        private readonly ILogger logger;

        public VillaController(IVillaRepository villaRepository, ILogger<VillaController> logger) {
            this.villaRepository = villaRepository;
            this.logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<VillaDTO>))]
        public IActionResult GetVillas() {
            var list = VillaStore.villaList;
            return Ok(list);
        }

        [HttpGet("id", Name = "GetVilla")]
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
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateVilla([FromBody] VillaDTO villa) {
            if (villa == null) {
                return BadRequest(villa);
            }
            if (villa.Id > 0) {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            if (VillaStore.villaList.Where(v => v.Name.ToLower() == villa.Name.ToLower()).FirstOrDefault() != null) {
                ModelState.AddModelError("Error", "Villa with this name already exists.");
                return BadRequest(ModelState);
            }
            if (ModelState.IsValid) {

                villa.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id + 1;
                VillaStore.villaList.Add(villa);
                return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
            }
            else {
                return BadRequest(ModelState);
            }
        }
        [HttpDelete("id")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteVilla(int id) {
            if (id == 0) {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if (villa == null) {
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);
            return NoContent();
        }

        [HttpPut("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDTO villa) {
            if (villa == null) {
                return BadRequest();
            }
            if (ModelState.IsValid) {
                var villaToUpdate = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
                if (villaToUpdate == null) {
                    return BadRequest();
                }
                VillaStore.villaList[VillaStore.villaList.FindIndex(x => x.Id == id)] = villa;
                return NoContent();
            }
            else {
                return BadRequest();
            }
        }

        [HttpPatch("id")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDTO> patch) {
            if (patch == null || id == 0) {
                return BadRequest();
            }
            if (ModelState.IsValid) {
                var villaToUpdate = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
                if (villaToUpdate == null) {
                    return BadRequest();
                }
                patch.ApplyTo(villaToUpdate, ModelState);
                return NoContent();
            }
            else {
                return BadRequest(ModelState);
            }
        }
    }
}
