using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class RentCategoryRepository : Repository, IRentCategoryRepository
    {
        public RentCategoryRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<RentCategory> GetAll()
        {
            return _context.RentCategories;
        }

        public RentCategory? Create(RentCategory rentCategory)
        {
            rentCategory = _context.RentCategories.Add(rentCategory).Entity;
            _context.SaveChanges();

            return rentCategory;
        }

        public RentCategory? Get(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id.Equals(id));
        }

        public RentCategory? Update(RentCategory rentCategory)
        {
            _context.RentCategories.Update(rentCategory);
            _context.SaveChanges();

            return rentCategory;
        }

        public void Delete(RentCategory rentCategory)
        {
            _context.RentCategories.Remove(rentCategory);
            _context.SaveChanges();
        }
    }
}
