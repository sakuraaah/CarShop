using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class StatusRepository : Repository, IStatusRepository
    {
        public StatusRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<Status> GetAll()
        {
            return _context.Statuses;
        }

        public Status? Create(Status status)
        {
            status = _context.Statuses.Add(status).Entity;
            _context.SaveChanges();

            return status;
        }

        public Status? Get(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id.Equals(id));
        }

        public Status? Update(Status status)
        {
            _context.Statuses.Update(status);
            _context.SaveChanges();

            return status;
        }

        public void Delete(Status status)
        {
            _context.Statuses.Remove(status);
            _context.SaveChanges();
        }
    }
}
