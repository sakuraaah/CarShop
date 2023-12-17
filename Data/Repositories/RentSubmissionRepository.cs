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

        public RentSubmissionListResponseDto GetList(RentSubmissionQueryDto query, bool isAdmin = false)
        {
            IQueryable<RentSubmission> rentSubmissionQuery = GetAll()
                .Include(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.Mark);

            if (!string.IsNullOrWhiteSpace(query.Username))
            {
                rentSubmissionQuery = rentSubmissionQuery.Where(x => x.User.UserName.Equals(query.Username));
            }
            if (!string.IsNullOrWhiteSpace(query.AplNr))
            {
                rentSubmissionQuery = rentSubmissionQuery.Where(x => x.AplNr.Equals(query.AplNr));
            }
            if (!string.IsNullOrWhiteSpace(query.RegNr))
            {
                rentSubmissionQuery = rentSubmissionQuery.Where(x => x.RegNr.Equals(query.RegNr));
            }
            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                rentSubmissionQuery = rentSubmissionQuery.Where(x => x.Category.Name.Contains(query.Category));
            }
            if (!string.IsNullOrWhiteSpace(query.Mark))
            {
                rentSubmissionQuery = rentSubmissionQuery.Where(x => x.Mark.Name.Contains(query.Mark));
            }
            if (!string.IsNullOrWhiteSpace(query.Model))
            {
                rentSubmissionQuery = rentSubmissionQuery.Where(x => x.Model.Contains(query.Model));
            }
            if (!string.IsNullOrWhiteSpace(query.Status))
            {
                rentSubmissionQuery = rentSubmissionQuery.Where(x => x.Status.Equals(query.Status));
            }
            if (isAdmin)
            {
                rentSubmissionQuery = rentSubmissionQuery.Where(x => x.Status != "Draft");
            }
            if(query.StartDate != null)
            {
                rentSubmissionQuery = rentSubmissionQuery.Where(x => x.Created >= query.StartDate);
            }
            if (query.EndDate != null)
            {
                rentSubmissionQuery = rentSubmissionQuery.Where(x => x.Created <= query.EndDate);
            }

            switch (query.orderDirection)
            {
                case "asc":
                    rentSubmissionQuery = rentSubmissionQuery.OrderBy(x => EF.Property<object>(x, query.orderBy));
                    break;
                case "desc":
                    rentSubmissionQuery = rentSubmissionQuery.OrderByDescending(x => EF.Property<object>(x, query.orderBy));
                    break;
                default:
                    // Default to ascending order if orderDirection is not specified or invalid
                    rentSubmissionQuery = rentSubmissionQuery.OrderBy(x => EF.Property<object>(x, query.orderBy));
                    break;
            }

            var totalCount = rentSubmissionQuery.Count();

            var rentSubmissions = rentSubmissionQuery
                .Select(x => new RentSubmissionListResponseItemDto(x))
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToArray()
                .ToList();


            return new RentSubmissionListResponseDto
            {
                Total = totalCount,
                Page = query.Page,
                PageSize = query.PageSize,
                Results = rentSubmissions
            };
        }

        public bool Exists(string aplNr, string regNr, int? id)
        {
            IQueryable<RentSubmission> rentSubmissionQuery = GetAll()
                .Where(x => x.AplNr.Equals(aplNr))
                .Where(x => x.RegNr.Equals(regNr))
                .Where(x => x.Status != "Cancelled");

            if (id != null)
            {
                rentSubmissionQuery = rentSubmissionQuery.Where(x => x.Id != id);
            }

            return (rentSubmissionQuery.Count() > 0);
        }

        public RentSubmissionResponseDto? Create(RentSubmission rentSubmission)
        {
            rentSubmission = _context.RentSubmissions.Add(rentSubmission).Entity;
            _context.SaveChanges();

            RentSubmissionResponseDto rentSubmissionResponseDto = new RentSubmissionResponseDto(rentSubmission);

            return rentSubmissionResponseDto;
        }

        public RentSubmission? Get(int id, ApplicationUser? user)
        {
            IQueryable<RentSubmission> rentSubmissionQuery = GetAll()
                .Include(x => x.AvailableStatusTransitions)
                .Where(x => x.Id.Equals(id));

            if (user != null)
            {
                rentSubmissionQuery = rentSubmissionQuery.Where(x => x.User.Equals(user));
            }
            else
            {
                rentSubmissionQuery = rentSubmissionQuery.Where(x => x.Status != "Draft");
            }

            return rentSubmissionQuery.FirstOrDefault();
        }

        public RentSubmissionResponseDto? GetItem(int id, ApplicationUser? user)
        {
            IQueryable<RentSubmission> rentSubmissionQuery = GetAll()
                .Include(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.Mark)
                .Include(x => x.AvailableStatusTransitions)
                .Where(x => x.Id.Equals(id));

            if (user != null)
            {
                rentSubmissionQuery = rentSubmissionQuery.Where(x => x.User.Equals(user));
            }

            return rentSubmissionQuery
                .Select(x => new RentSubmissionResponseDto(x))
                .FirstOrDefault();
        }

        public RentSubmissionResponseDto? Update(RentSubmission rentSubmission)
        {
            _context.RentSubmissions.Update(rentSubmission);
            _context.SaveChanges();

            RentSubmissionResponseDto rentSubmissionResponseDto = new RentSubmissionResponseDto(rentSubmission);

            return rentSubmissionResponseDto;
        }

        public void Delete(RentSubmission rentSubmission)
        {
            _context.RentSubmissions.Remove(rentSubmission);
            _context.SaveChanges();
        }
    }
}
