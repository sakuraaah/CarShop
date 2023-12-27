using CarShop.Dtos;
using CarShop.Models;

namespace CarShop.Data
{
    public interface IBuyItemRepository
    {
        IQueryable<BuyItem> GetAll();
        BuyItemListResponseDto GetList(BuyItemQueryDto query, ApplicationUser? user, string userRole = "Public");
        bool Exists(string aplNr, string regNr, int? id);
        BuyItemResponseDto? Create(BuyItem buyItem);
        BuyItem? Get(int id, ApplicationUser? user, string userRole = "Buyer");
        BuyItemResponseDto? GetItem(int id, ApplicationUser? user, string userRole = "Public");
        BuyItemResponseDto? Update(BuyItem buyItem);
        void Delete(BuyItem buyItem);
    }
}
