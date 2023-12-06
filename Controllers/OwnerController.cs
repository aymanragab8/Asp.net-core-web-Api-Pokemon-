using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Wep_Api.Dto;
using Pokemon_Wep_Api.interfaces;
using Pokemon_Wep_Api.Models;
using Pokemon_Wep_Api.Repository;

namespace Pokemon_Wep_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;
        private readonly IMapper _mapper;
        private readonly ICountryRpository _countryRpository;

        public OwnerController(IOwnerRepository ownerRepository, IMapper mapper , ICountryRpository countryRpository)
        {
            _ownerRepository = ownerRepository;
            _mapper = mapper;
            _countryRpository = countryRpository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var Owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(Owners);
        }

        [HttpGet("{OwnerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        public IActionResult GetOwner(int OwnerId)
        {
            if (!_ownerRepository.OwnerExists(OwnerId))
                return NotFound();
            var owner = _mapper.Map<OwnerDto>(_ownerRepository.GetOwner(OwnerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(owner);
        }

        [HttpGet("Pokemon/{OwnerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetPokemonsByOwnerId(int OwnerId)
        {

            if (!_ownerRepository.OwnerExists(OwnerId))
                return NotFound();

            var owner = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(OwnerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(owner);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId ,[FromBody] OwnerDto ownerCreate)
        {
            //if input is null
            if (ownerCreate == null)
                return BadRequest(ModelState);
            //if input is not null and obtained all of data ,  Does it really exist?
            var owner = _ownerRepository.GetOwners().Where(c => c.LastName.Trim().ToUpper() == ownerCreate.LastName.TrimEnd().ToUpper()).FirstOrDefault();
            if (owner != null)
            {
                ModelState.AddModelError("", "The Owner Already Exists");
                return StatusCode(422, ModelState);
            }
            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var ownerMap = _mapper.Map<Owner>(ownerCreate);
            ownerMap.Country = _countryRpository.GetCountry(countryId);

            //if input is not null and obtained all of data , is it right saved ?
            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            //no problems ? => ok 
            return Ok("Successfully Created");
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateOwner(int ownerId, [FromBody] OwnerDto ownerUpdate)
        {   // id is empty !
            if (ownerUpdate == null)
                return BadRequest(ModelState);
            //IDs are not equals !

            if (ownerId != ownerUpdate.Id)
                return BadRequest(ModelState);

            // input Id is not exist in Db
            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();

            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if input is not null and obtained all of data , is it right saved ?
            var ownerMap = _mapper.Map<Owner>(ownerUpdate);
            
            if (!_ownerRepository.UpdateOwner(ownerMap))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            //no problems ? => ok 
            return Ok("Successfully Updated");
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteOwner(int ownerId)
        {
            // input Id is not exist in Db
            if (!_ownerRepository.OwnerExists(ownerId))
                return NotFound();

            //found Id
            var owner = _ownerRepository.GetOwner(ownerId);

            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_ownerRepository.DeleteOwner(owner))
                ModelState.AddModelError("", "something went wrong while deleting");

            //no problems ? => ok 
            return Ok("Successfully deleted");
        }
    }
}
