namespace CarShop.Data
{
    public interface IStatusRepository
    {
        IQueryable<Status> GetAll();
        Status? Create(Status status);
        Status? Get(int id);
        Status? Update(Status status);
        void Delete(Status status);
    }
}
