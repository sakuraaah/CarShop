namespace CarShop.Data
{
    public interface ICarClassRepository
    {
        IQueryable<CarClass> GetAll();
        CarClass? Create(CarClass carClass);
        CarClass? Get(int id);
        CarClass? Update(CarClass carClass);
        void Delete(CarClass carClass);
    }
}
