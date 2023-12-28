using CarShop.Dtos;
using CarShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class RentOrderRepository : Repository, IRentOrderRepository
    {
        public RentOrderRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<RentOrder> GetAll(ApplicationUser user)
        {
            return _context.RentOrders.Where(x => x.User == user);
        }

        public RentOrder? Get(int id, ApplicationUser user)
        {
            return GetAll(user)
                .Where(x => x.Id.Equals(id))
                .FirstOrDefault();
        }

        public List<RentOrderResponseDto> GetList(ApplicationUser user)
        {
            return GetAll(user)
                .Include(x => x.RentItem)
                    .ThenInclude(i => i.Mark)
                .OrderByDescending(x => x.Status)
                .ThenByDescending(x => x.StartTime)
                .Select(x => new RentOrderResponseDto(x))
                .ToArray()
                .ToList();
        }

        public RentOrderResponseDto? Create(RentOrder rentOrder)
        {
            rentOrder = _context.RentOrders.Add(rentOrder).Entity;
            _context.SaveChanges();

            RentOrderResponseDto rentOrderResponseDto = new RentOrderResponseDto(rentOrder);

            return rentOrderResponseDto;
        }

        public RentOrderResponseDto? Update(RentOrder rentOrder)
        {
            _context.RentOrders.Update(rentOrder);
            _context.SaveChanges();

            RentOrderResponseDto rentOrderResponseDto = new RentOrderResponseDto(rentOrder);

            return rentOrderResponseDto;
        }
    }
}
