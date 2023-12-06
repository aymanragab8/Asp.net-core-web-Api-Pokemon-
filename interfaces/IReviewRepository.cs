using Pokemon_Wep_Api.Models;
using System.Collections.Generic;

namespace Pokemon_Wep_Api.interfaces
{
    public interface IReviewRepository
    {
        ICollection<Review>GetReviews();
        Review GetReview(int reviewid);
        ICollection<Review> GetReviewsOfAPokemon(int PokeId);
        bool ReviewExists(int reviewid);
        bool CreateReview(Review review);
        bool UpdateReview(Review review);
        bool DeleteReview(Review review);
        bool DeleteReviews(List<Review> reviews);
        bool Save();
    }
}
