using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Wep_Api.Data;
using Pokemon_Wep_Api.Dto;
using Pokemon_Wep_Api.interfaces;
using Pokemon_Wep_Api.Models;
using Pokemon_Wep_Api.Repository;

namespace Pokemon_Wep_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;
        private readonly IReviewRepository _reviewRepository;

        public PokemonController(IPokemonRepository pokemonRepository ,IMapper mapper , IReviewRepository reviewRepository)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        [ProducesResponseType (200,Type = typeof (IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons =_mapper.Map<List<PokemonDto>>(_pokemonRepository.GetPokemons());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);
        }

        [HttpGet("{PokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        public IActionResult GetPokemon(int PokeId) 
        {
            if (!_pokemonRepository.PokemonExists(PokeId))
                return NotFound();
            var pokemon = _mapper.Map<PokemonDto>(_pokemonRepository.GetPokemon(PokeId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemon);
        }

        [HttpGet("{PokeId}/ratng")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        public IActionResult GetPokemonRating(int PokeId)
        {
            if (!_pokemonRepository.PokemonExists(PokeId))
                return NotFound();
            var rating = _pokemonRepository.GetPokemonRating(PokeId);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int catId, [FromBody] PokemonDto pokemonCreate)
        {
            //if input is null
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            //if input is not null and obtained all of data ,  Does it really exist?
            var pokemons = _pokemonRepository.GetPokemonTrimToUpper(pokemonCreate);
            if (pokemons != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }

            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if input is not null and obtained all of data , is it right saved ?
            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);
            if (!_pokemonRepository.CreatePokemon(ownerId, catId, pokemonMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            //no problems ? => ok 
            return Ok("Successfully created");
        }

        [HttpPut("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId,int pokemonId, [FromBody] PokemonDto pokemonUpdate)
        {   // id is empty !
            if (pokemonUpdate == null)
                return BadRequest(ModelState);
            //IDs are not equals !

            if (pokemonId != pokemonUpdate.Id)
                return BadRequest(ModelState);

            // input Id is not exist in Db
            if (!_pokemonRepository.PokemonExists(pokemonId))
                return NotFound();

            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if input is not null and obtained all of data , is it right saved ?
            var pokemonMap = _mapper.Map<Pokemon>(pokemonUpdate);

            if (!_pokemonRepository.UpdatePokemon(ownerId,categoryId,pokemonMap))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            //no problems ? => ok 
            return Ok("Successfully Updated");
        }

        [HttpDelete("{pokemonId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeletePokemon(int pokemonId)
        {
            // input Id is not exist in Db
            if (!_pokemonRepository.PokemonExists(pokemonId))
                return NotFound();

            //found Id
            var reviews =_reviewRepository.GetReviewsOfAPokemon(pokemonId);
            var pokemon = _pokemonRepository.GetPokemon(pokemonId);

            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_reviewRepository.DeleteReviews(reviews.ToList()))
                ModelState.AddModelError("", "something went wrong while deleting");
            
            if (!_pokemonRepository.DeletePokemon(pokemon))
                ModelState.AddModelError("", "something went wrong while deleting");

            //no problems ? => ok 
            return Ok("Successfully deleted");
        }
    }
}
