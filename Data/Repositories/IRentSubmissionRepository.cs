using CarShop.Dtos;
using CarShop.Models;

namespace CarShop.Data
{
    public interface IRentSubmissionRepository
    {
        IQueryable<RentSubmission> GetAll();
        RentSubmissionListResponseDto GetList(RentSubmissionQueryDto query, bool isAdmin = false);
        bool Exists(string aplNr, string regNr, int? id);
        RentSubmissionResponseDto? Create(RentSubmission rentSubmission);
        RentSubmission? Get(int id, ApplicationUser? user);
        RentSubmissionResponseDto? GetItem(int id, ApplicationUser? user);
        RentSubmissionResponseDto? Update(RentSubmission rentSubmission);
        void Delete(RentSubmission rentSubmission);
    }
}
