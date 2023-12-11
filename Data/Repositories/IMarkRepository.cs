namespace CarShop.Data
{
    public interface IMarkRepository
    {
        IQueryable<Mark> GetAll();
        Mark? Create(Mark mark);
        Mark? Get(int id);
        Mark? Update(Mark mark);
        void Delete(Mark mark);
    }
}
