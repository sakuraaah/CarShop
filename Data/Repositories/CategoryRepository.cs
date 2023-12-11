using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class CategoryRepository : Repository, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<Category> GetAll()
        {
            return _context.Categories;
        }

        public Category? Create(Category category)
        {
            category = _context.Categories.Add(category).Entity;
            _context.SaveChanges();

            return category;
        }

        public Category? Get(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id.Equals(id));
        }

        public Category? Update(Category category)
        {
            _context.Categories.Update(category);
            _context.SaveChanges();

            return category;
        }

        public void Delete(Category category)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
        }
    }
}
