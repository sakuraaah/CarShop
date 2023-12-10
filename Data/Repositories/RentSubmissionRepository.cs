using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class RentSubmissionRepository : Repository, IRentSubmissionRepository
    {
        public RentSubmissionRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<RentSubmission> GetAll()
        {
            return _context.RentSubmissions;
        }

        public RentSubmission? Create(RentSubmission rentSubmission)
        {
            rentSubmission = _context.RentSubmissions.Add(rentSubmission).Entity;
            _context.SaveChanges();

            return rentSubmission;
        }

        public RentSubmission? Get(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id.Equals(id));
        }

        public RentSubmission? Update(RentSubmission rentSubmission)
        {
            _context.RentSubmissions.Update(rentSubmission);
            _context.SaveChanges();

            return rentSubmission;
        }

        public void Delete(RentSubmission rentSubmission)
        {
            _context.RentSubmissions.Remove(rentSubmission);
            _context.SaveChanges();
        }
    }
}
