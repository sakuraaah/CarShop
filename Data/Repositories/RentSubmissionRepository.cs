using CarShop.Dtos;
using CarShop.Models;
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

        public IQueryable<RentSubmissionListDto> GetSellerList(ApplicationUser user)
        {
            return GetAll()
                .Include(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.Mark)
                .Where(x => x.User.Equals(user))
                .Select(x => new RentSubmissionListDto
                {
                    Id = x.Id,
                    Created = x.Created,
                    AplNr = x.AplNr,
                    RegNr = x.RegNr,
                    Category = x.Category.Name,
                    Mark = x.Mark.Name,
                    Model = x.Model,
                    Status = x.Status,
                });
        }

        public IQueryable<RentSubmissionListDto> GetAdminList()
        {
            return GetAll()
                .Include(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.Mark)
                .Where(x => x.Status != "Draft")
                .Select(x => new RentSubmissionListDto
                {
                    Id = x.Id,
                    Created = x.Created,
                    User =
                    {
                        Id = x.User.Id,
                        FirstName = x.User.FirstName,
                        LastName = x.User.LastName,
                        UserName = x.User.UserName,
                    },
                    AplNr = x.AplNr,
                    RegNr = x.RegNr,
                    Category = x.Category.Name,
                    Mark = x.Mark.Name,
                    Model = x.Model,
                    Status = x.Status,
                });
        }

        public RentSubmission? Create(RentSubmission rentSubmission)
        {
            rentSubmission = _context.RentSubmissions.Add(rentSubmission).Entity;
            _context.SaveChanges();

            return rentSubmission;
        }

        public RentSubmission? Get(int id, ApplicationUser? user)
        {
            IQueryable<RentSubmission> rentSubmissionQuery = GetAll()
                .Where(x => x.Id.Equals(id));

            if (user != null)
            {
                rentSubmissionQuery.Where(x => x.User.Equals(user));
            }

            return rentSubmissionQuery.FirstOrDefault();
        }

        public RentSubmission? GetForSeller(int id, ApplicationUser user)
        {
            return GetAll()
                .Include(x => x.Category)
                .Include(x => x.Mark)
                .Include(x => x.AvailableStatusTransitions)
                .Where(x => x.Id.Equals(id))
                .Where(x => x.User.Equals(user))
                .FirstOrDefault();
        }

        public RentSubmission? GetForAdmin(int id)
        {
            return GetAll()
                .Include(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.Mark)
                .Include(x => x.AvailableStatusTransitions)
                .Where(x => x.Id.Equals(id))
                .FirstOrDefault();
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
