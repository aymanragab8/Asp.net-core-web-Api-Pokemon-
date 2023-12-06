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
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;
        private readonly IMapper _mapper;

        public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper)
        {
            _reviewerRepository = reviewerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        public IActionResult GetReviewers()
        {
            var Reviewers = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewers());
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(Reviewers);
        }

        [HttpGet("{ReviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        public IActionResult GetReviewer(int ReviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(ReviewerId))
                return NotFound();
            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(ReviewerId));
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(reviewer);
        }

        [HttpGet("Review/{ReviewerId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        public IActionResult GetReviewsByAReviewer(int ReviewerId)
        {
            if (!_reviewerRepository.ReviewerExists(ReviewerId))
                return NotFound();

            var reviews = _mapper.Map<List<ReviewDto>>(_reviewerRepository.GetReviewsByReviewer(ReviewerId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewer([FromBody] ReviewerDto reviewerCreate)
        {
            //if input is null
            if (reviewerCreate == null)
                return BadRequest(ModelState);
            //if input is not null and obtained all of data ,  Does it really exist?
            var reviewer = _reviewerRepository.GetReviewers().Where(c => c.LastName.Trim().ToUpper() == reviewerCreate.LastName.TrimEnd().ToUpper()).FirstOrDefault();
            if (reviewer != null)
            {
                ModelState.AddModelError("", "The Reviewer Already Exists");
                return StatusCode(422, ModelState);
            }
            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);
            //if input is not null and obtained all of data , is it right saved ?
            if (!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500, ModelState);
            }
            //no problems ? => ok 
            return Ok("Successfully Created");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateReviewer(int reviewerId, [FromBody] ReviewerDto reviewerUpdate)
        {   // id is empty !
            if (reviewerUpdate == null)
                return BadRequest(ModelState);

            //IDs are not equals !
            if (reviewerId != reviewerUpdate.Id)
                return BadRequest(ModelState);

            // input Id is not exist in Db
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            //if input is not null and obtained all of data , is it right saved ?
            var reviewerMap = _mapper.Map<Reviewer>(reviewerUpdate);

            if (!_reviewerRepository.UpdateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            //no problems ? => ok 
            return Ok("Successfully Updated");
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteReviewer(int reviewerId)
        {
            // input Id is not exist in Db
            if (!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            //found Id
            var reviewer = _reviewerRepository.GetReviewer(reviewerId);

            //if input is not null and obtained all of data , is it a valid data ?
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_reviewerRepository.DeleteReviewer(reviewer))
                ModelState.AddModelError("", "something went wrong while deleting");

            //no problems ? => ok 
            return Ok("Successfully deleted");
        }
    }
}