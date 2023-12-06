using Pokemon_Wep_Api.Data;
using Pokemon_Wep_Api.interfaces;
using Pokemon_Wep_Api.Models;

namespace Pokemon_Wep_Api.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateReview(Review review)
        {
            _context.Reviews.Add(review);
            return Save();
        }

        public bool DeleteReview(Review review)
        {
            _context.Remove(review);
            return Save();
        }

        public bool DeleteReviews(List<Review> reviews)
        {
            _context.RemoveRange(reviews);
            return Save();
        }

        public Review GetReview(int reviewid)
        {
            return _context.Reviews.Where(r => r.Id == reviewid).FirstOrDefault();
        }

        public ICollection<Review> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        public ICollection<Review> GetReviewsOfAPokemon(int PokeId)
        {
            return _context.Reviews.Where(p =>p.Pokemon.Id==PokeId).ToList();
        }

        public bool ReviewExists(int reviewid)
        {
            return _context.Reviews.Any(r => r.Id == reviewid);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReview(Review review)
        {
           _context.Update(review);
            return Save();
        }
    }
}
