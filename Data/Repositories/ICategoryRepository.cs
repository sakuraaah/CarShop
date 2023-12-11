namespace CarShop.Data
{
    public interface ICategoryRepository
    {
        IQueryable<Category> GetAll();
        Category? Create(Category category);
        Category? Get(int id);
        Category? Update(Category category);
        void Delete(Category category);
    }
}
