using CarShop.Dtos;
using CarShop.Models;

namespace CarShop.Data
{
    public interface IRentOrderRepository
    {
        IQueryable<RentOrder> GetAll(ApplicationUser user);
        RentOrder? Get(int id, ApplicationUser user);
        List<RentOrderResponseDto> GetList(ApplicationUser user);
        RentOrderResponseDto? Create(RentOrder rentOrder);
        RentOrderResponseDto? Update(RentOrder rentOrder);
    }
}
