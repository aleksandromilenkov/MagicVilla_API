using MagicVilla_VillaAPI.DTO;
using MagicVilla_VillaAPI.Interface;
using MagicVilla_VillaAPI.Models;
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
            var list = villaRepository.GetAllVillas();
            return Ok(list);
        }

        [HttpGet("id", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetVillaById(int id) {
            if (id <= 0) {
                logger.LogError("Get Villa Error with id " + id);
                return BadRequest();
            }
            if (id == null) {
                return NotFound();
            }
            var villa = villaRepository.GetVillaById(id);

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
            if (villaRepository.CheckVillaExistance(villa.Name)) {
                ModelState.AddModelError("Error", "Villa with this name already exists.");
                return BadRequest(ModelState);
            }
            if (ModelState.IsValid) {
                var isVillaCreated = villaRepository.CreateVilla(new Villa() {
                    Amenty = villa.Amenty,
                    Details = villa.Details,
                    ImageUrl = villa.ImageUrl,
                    Name = villa.Name,
                    Occupancy = villa.Occupancy,
                    Rate = villa.Rate,
                    Sqft = villa.Sqft
                });
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
            var villa = villaRepository.GetVillaById(id);
            if (villa == null) {
                return NotFound();
            }
            var isVillaDeleted = villaRepository.DeleteVilla(villa);
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
                var villaToUpdate = villaRepository.GetVillaByIdAsNoTracking(id);
                if (villaToUpdate == null) {
                    return BadRequest();
                }
                var isVillaUpdated = villaRepository.UpdateVilla(new Villa() {
                    Id = villa.Id,
                    Amenty = villa.Amenty,
                    Details = villa.Details,
                    ImageUrl = villa.ImageUrl,
                    Name = villa.Name,
                    Occupancy = villa.Occupancy,
                    Rate = villa.Rate,
                    Sqft = villa.Sqft
                });
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
                var villaToUpdate = villaRepository.GetVillaByIdAsNoTracking(id);
                if (villaToUpdate == null) {
                    return BadRequest();
                }
                VillaDTO villaDTO = new VillaDTO() {
                    Id = villaToUpdate.Id,
                    Amenty = villaToUpdate.Amenty,
                    Details = villaToUpdate.Details,
                    ImageUrl = villaToUpdate.ImageUrl,
                    Name = villaToUpdate.Name,
                    Occupancy = villaToUpdate.Occupancy,
                    Rate = villaToUpdate.Rate,
                    Sqft = villaToUpdate.Sqft
                };

                patch.ApplyTo(villaDTO, ModelState);
                Villa model = new Villa() {
                    Id = villaDTO.Id,
                    Amenty = villaDTO.Amenty,
                    Details = villaDTO.Details,
                    ImageUrl = villaDTO.ImageUrl,
                    Name = villaDTO.Name,
                    Occupancy = villaDTO.Occupancy,
                    Rate = villaDTO.Rate,
                    Sqft = villaDTO.Sqft
                };
                var isVillaUpdated = villaRepository.UpdateVilla(model);
                return NoContent();
            }
            else {
                return BadRequest(ModelState);
            }
        }
    }
}
