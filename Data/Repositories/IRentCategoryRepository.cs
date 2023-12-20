namespace CarShop.Data
{
    public interface IRentCategoryRepository
    {
        IQueryable<RentCategory> GetAll();
        RentCategory? Create(RentCategory rentCategory);
        RentCategory? Get(int id);
        RentCategory? Update(RentCategory rentCategory);
        void Delete(RentCategory rentCategory);
    }
}
