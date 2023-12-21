using CarShop.Dtos;
using CarShop.Models;

namespace CarShop.Data
{
    public interface IBuyItemRepository
    {
        IQueryable<BuyItem> GetAll();
        BuyItemListResponseDto GetList(BuyItemQueryDto query, string userRole = "Buyer");
        bool Exists(string aplNr, string regNr, int? id);
        BuyItemResponseDto? Create(BuyItem buyItem);
        BuyItem? Get(int id, ApplicationUser? user);
        BuyItemResponseDto? GetItem(int id, ApplicationUser? user, string userRole = "Buyer");
        BuyItemResponseDto? Update(BuyItem buyItem);
        void Delete(BuyItem buyItem);
    }
}
