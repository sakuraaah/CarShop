using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class BodyTypeRepository : Repository, IBodyTypeRepository
    {
        public BodyTypeRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<BodyType> GetAll()
        {
            return _context.BodyTypes;
        }

        public BodyType? Create(BodyType bodyType)
        {
            bodyType = _context.BodyTypes.Add(bodyType).Entity;
            _context.SaveChanges();

            return bodyType;
        }

        public BodyType? Get(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id.Equals(id));
        }

        public BodyType? Update(BodyType bodyType)
        {
            _context.BodyTypes.Update(bodyType);
            _context.SaveChanges();

            return bodyType;
        }

        public void Delete(BodyType bodyType)
        {
            _context.BodyTypes.Remove(bodyType);
            _context.SaveChanges();
        }
    }
}
