namespace CarShop.Data
{
    public interface IColorRepository
    {
        IQueryable<Color> GetAll();
        Color? Create(Color color);
        Color? Get(int id);
        Color? Update(Color color);
        void Delete(Color color);
    }
}
