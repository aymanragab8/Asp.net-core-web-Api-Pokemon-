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
    public class CountryController : ControllerBase
    {
        private readonly ICountryRpository _countryRpository;
        private readonly IMapper _mapper;

        public CountryController(ICountryRpository countryRpository ,IMapper mapper)
        {
            _countryRpository = countryRpository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200,Type =typeof(IEnumerable<Country>))]
        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRpository.GetCountries());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(countries);
        }

        [HttpGet("{CountryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult GetCountry(int CountryId)
        {
            if (!_countryRpository.CountryExists(CountryId))
                return NotFound();
            var country = _mapper.Map<CountryDto>(_countryRpository.GetCountry(CountryId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(country);
        }

        [HttpGet("Countries/{OwnerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        public IActionResult GetCountryOfAnOwner(int ownerId)
        {
          
            var country = _mapper.Map<CountryDto>(_countryRpository.GetCountryByOwner(ownerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(country);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
        {
            //if input is null
            if (countryCreate == null)
                return BadRequest(ModelState);
            //if input is not null and obtained all of data ,  Does it really exist?
            var country = _countryRpository.GetCountries().Where(c => c.Name.Trim().ToUpper() == countryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if (country != null)
            {
                ModelState.AddModelError("", "The Country Already Exists");
                return StatusCode(422, ModelState);
            }
            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var countryMap = _mapper.Map<Country>(countryCreate);
            //if input is not null and obtained all of data , is it right saved ?
            if (!_countryRpository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            //no problems ? => ok 
            return Ok("Successfully Created");
        }

        [HttpPut("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCountry(int countryId, [FromBody] CountryDto countryUpdate)
        {   // id is empty !
            if (countryUpdate == null)
                return BadRequest(ModelState);
            //IDs are not equals !

            if (countryId != countryUpdate.Id)
                return BadRequest(ModelState);

            // input Id is not exist in Db
            if (!_countryRpository.CountryExists(countryId))
                return NotFound();

            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if input is not null and obtained all of data , is it right saved ?
            var countryMap = _mapper.Map<Country>(countryUpdate);
            
            if (!_countryRpository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            //no problems ? => ok 
            return Ok("Successfully Updated");
        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            // input Id is not exist in Db
            if (!_countryRpository.CountryExists(countryId))
                return NotFound();

            //found Id
            var country = _countryRpository.GetCountry(countryId);

            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_countryRpository.DeleteCountry(country))
                ModelState.AddModelError("", "something went wrong while deleting");

            //no problems ? => ok 
            return Ok("Successfully deleted");
        }



    }
}
