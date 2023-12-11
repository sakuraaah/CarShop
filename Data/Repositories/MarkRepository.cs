using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class MarkRepository : Repository, IMarkRepository
    {
        public MarkRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<Mark> GetAll()
        {
            return _context.Marks;
        }

        public Mark? Create(Mark mark)
        {
            mark = _context.Marks.Add(mark).Entity;
            _context.SaveChanges();

            return mark;
        }

        public Mark? Get(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id.Equals(id));
        }

        public Mark? Update(Mark mark)
        {
            _context.Marks.Update(mark);
            _context.SaveChanges();

            return mark;
        }

        public void Delete(Mark mark)
        {
            _context.Marks.Remove(mark);
            _context.SaveChanges();
        }
    }
}
