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
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewController(IReviewRepository reviewRepository , IMapper mapper , IPokemonRepository pokemonRepository , IReviewerRepository reviewerRepository)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _pokemonRepository = pokemonRepository;
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        public IActionResult GetReviews()
        {
            var Reviews = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviews());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(Reviews);
        }

        [HttpGet("{ReviewId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        public IActionResult GetReview(int ReviewId)
        {
            if (!_reviewRepository.ReviewExists(ReviewId))
                return NotFound();
            var review = _mapper.Map<ReviewDto>(_reviewRepository.GetReview(ReviewId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(review);
        }

        [HttpGet("Pokemon/{PokeId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        public IActionResult GetReviewsForAPokemon(int PokeId)
        {
            var review = _mapper.Map<List<ReviewDto>>(_reviewRepository.GetReviewsOfAPokemon(PokeId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(review);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReview([FromQuery] int pokeId , [FromQuery] int reviewerId,[FromBody] ReviewDto reviewCreate)
        {
            //if input is null
            if (reviewCreate == null)
                return BadRequest(ModelState);
            //if input is not null and obtained all of data ,  Does it really exist?
            var reviews = _reviewRepository.GetReviews().Where(c => c.Title.Trim().ToUpper() == reviewCreate.Title.TrimEnd().ToUpper()).FirstOrDefault();
            if (reviews != null)
            {
                ModelState.AddModelError("", "The Review Already Exists");
                return StatusCode(422, ModelState);
            }
            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewMap = _mapper.Map<Review>(reviewCreate);

            reviewMap.Pokemon = _pokemonRepository.GetPokemon(pokeId);
            reviewMap.Reviewer = _reviewerRepository.GetReviewer(reviewerId);

            //if input is not null and obtained all of data , is it right saved ?
            if (!_reviewRepository.CreateReview(reviewMap))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            //no problems ? => ok 
            return Ok("Successfully Created");
        }

        [HttpPut("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReview(int reviewId, [FromBody] ReviewDto reviewUpdate)
        {   // id is empty !
            if (reviewUpdate == null)
                return BadRequest(ModelState);
            //IDs are not equals !

            if (reviewId != reviewUpdate.Id)
                return BadRequest(ModelState);

            // input Id is not exist in Db
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if input is not null and obtained all of data , is it right saved ?
            var reviewMap = _mapper.Map<Review>(reviewUpdate);

            if (!_reviewRepository.UpdateReview(reviewMap))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            //no problems ? => ok 
            return Ok("Successfully Updated");
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReview(int reviewId)
        {
            // input Id is not exist in Db
            if (!_reviewRepository.ReviewExists(reviewId))
                return NotFound();

            //found Id
            var review = _reviewRepository.GetReview(reviewId);

            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewRepository.DeleteReview(review))
                ModelState.AddModelError("", "something went wrong while deleting");

            //no problems ? => ok 
            return Ok("Successfully deleted");
        }
    }
}
