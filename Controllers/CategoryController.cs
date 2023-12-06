using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Pokemon_Wep_Api.Dto;
using Pokemon_Wep_Api.interfaces;
using Pokemon_Wep_Api.Models;
using Pokemon_Wep_Api.Repository;

namespace Pokemon_Wep_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var Categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(Categories);
        }

        [HttpGet("{CatId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        public IActionResult GetCategory(int CatId)
        {
            if (!_categoryRepository.CategoriesExists(CatId))
                return NotFound();
            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(CatId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(category);
        }

        [HttpGet("Pokemon/{CatId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetPokemonsByCategoryId(int CatId)
        {
            var pokemons = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonsByCategory(CatId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            //if input is null
            if (categoryCreate == null)
                return BadRequest(ModelState);
            //if input is not null and obtained all of data ,  Does it really exist?
            var category = _categoryRepository.GetCategories().Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();
            if (category != null)
            {
                ModelState.AddModelError("", "The Category Already Exists");
                return StatusCode(422, ModelState);
            }
            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = _mapper.Map<Category>(categoryCreate);
            //if input is not null and obtained all of data , is it right saved ?
            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            //no problems ? => ok 
            return Ok("Successfully Created");
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId,[FromBody] CategoryDto categoryUpdate)
        {   // id is empty !
            if(categoryUpdate==null)
                return BadRequest(ModelState);

            //IDs are not equals !
            if(categoryId!=categoryUpdate.Id)
                return BadRequest(ModelState);

            // input Id is not exist in Db
            if(!_categoryRepository.CategoriesExists(categoryId))
                return NotFound();

            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if input is not null and obtained all of data , is it right saved ?
            var categoryMap = _mapper.Map<Category>(categoryUpdate);
            
            if (!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            //no problems ? => ok 
            return Ok("Successfully Updated");
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        { 
            // input Id is not exist in Db
            if (!_categoryRepository.CategoriesExists(categoryId))
                return NotFound();

            //found Id
            var category = _categoryRepository.GetCategory(categoryId);

            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRepository.DeleteCategory(category))
                ModelState.AddModelError("", "something went wrong while deleting");

            //no problems ? => ok 
            return Ok("Successfully deleted");
        }

    }
}
