using Microsoft.EntityFrameworkCore;
using Pokemon_Wep_Api.Data;
using Pokemon_Wep_Api.interfaces;
using Pokemon_Wep_Api.Models;

namespace Pokemon_Wep_Api.Repository
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool CreateReviewer(Reviewer reviewer)
        {
            _context.Add(reviewer);
            return Save();
        }

        public bool DeleteReviewer(Reviewer reviewer)
        {
            _context.Remove(reviewer);
            return Save();
        }

        public Reviewer GetReviewer(int reviewerid)
        {
            return _context.Reviewers.Where(r => r.Id == reviewerid).Include(i =>i.Reviews).FirstOrDefault();
        }

        public ICollection<Reviewer> GetReviewers()
        {
            return _context.Reviewers.ToList();
        }

        public ICollection<Review> GetReviewsByReviewer(int reviewerid)
        {
            return _context.Reviews.Where(r=>r.Reviewer.Id==reviewerid).ToList();
        }

        public bool ReviewerExists(int reviewerid)
        {
            return _context.Reviewers.Any(r => r.Id == reviewerid);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateReviewer(Reviewer reviewer)
        {
            _context.Update(reviewer);
            return Save();
        }
    }
}
