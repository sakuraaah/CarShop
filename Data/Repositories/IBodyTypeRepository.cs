namespace CarShop.Data
{
    public interface IBodyTypeRepository
    {
        IQueryable<BodyType> GetAll();
        BodyType? Create(BodyType bodyType);
        BodyType? Get(int id);
        BodyType? Update(BodyType bodyType);
        void Delete(BodyType bodyType);
    }
}
