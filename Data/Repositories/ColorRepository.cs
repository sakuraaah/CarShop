using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class ColorRepository : Repository, IColorRepository
    {
        public ColorRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<Color> GetAll()
        {
            return _context.Colors;
        }

        public Color? Create(Color color)
        {
            color = _context.Colors.Add(color).Entity;
            _context.SaveChanges();

            return color;
        }

        public Color? Get(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id.Equals(id));
        }

        public Color? Update(Color color)
        {
            _context.Colors.Update(color);
            _context.SaveChanges();

            return color;
        }

        public void Delete(Color color)
        {
            _context.Colors.Remove(color);
            _context.SaveChanges();
        }
    }
}
