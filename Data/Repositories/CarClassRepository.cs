using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class CarClassRepository : Repository, ICarClassRepository
    {
        public CarClassRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<CarClass> GetAll()
        {
            return _context.CarClasses;
        }

        public CarClass? Create(CarClass carClass)
        {
            carClass = _context.CarClasses.Add(carClass).Entity;
            _context.SaveChanges();

            return carClass;
        }

        public CarClass? Get(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id.Equals(id));
        }

        public CarClass? Update(CarClass carClass)
        {
            _context.CarClasses.Update(carClass);
            _context.SaveChanges();

            return carClass;
        }

        public void Delete(CarClass carClass)
        {
            _context.CarClasses.Remove(carClass);
            _context.SaveChanges();
        }
    }
}
