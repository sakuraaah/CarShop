using CarShop.Dtos;
using CarShop.Models;

namespace CarShop.Data
{
    public interface IRentItemRepository
    {
        IQueryable<RentItem> GetAll();
        RentItemListResponseDto GetList(RentItemQueryDto query, string userRole = "Public");
        bool Exists(int rentSubmissionId, int? id);
        RentItemResponseDto? Create(RentItem rentItem);
        RentItem? Get(int id, ApplicationUser? user, string userRole = "Buyer");
        RentItemResponseDto? GetItem(int id, ApplicationUser? user, string userRole = "Public");
        RentItemResponseDto? Update(RentItem rentItem);
        void Delete(RentItem rentItem);
    }
}
