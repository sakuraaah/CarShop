using CarShop.Dtos;
using CarShop.Models;

namespace CarShop.Data
{
    public interface IRentSubmissionRepository
    {
        IQueryable<RentSubmission> GetAll();
        IQueryable<RentSubmissionListDto> GetSellerList(ApplicationUser user);
        IQueryable<RentSubmissionListDto> GetAdminList();
        RentSubmission? Create(RentSubmission rentSubmission);
        RentSubmission? Get(int id, ApplicationUser? user);
        RentSubmission? GetForSeller(int id, ApplicationUser user);
        RentSubmission? GetForAdmin(int id);
        RentSubmission? Update(RentSubmission rentSubmission);
        void Delete(RentSubmission rentSubmission);
    }
}
