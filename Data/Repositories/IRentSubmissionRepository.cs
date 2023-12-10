namespace CarShop.Data
{
    public interface IRentSubmissionRepository
    {
        IQueryable<RentSubmission> GetAll();
        RentSubmission? Create(RentSubmission rentSubmission);
        RentSubmission? Get(int id);
        RentSubmission? Update(RentSubmission rentSubmission);
        void Delete(RentSubmission rentSubmission);
    }
}
