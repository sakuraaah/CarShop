using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class FeatureRepository : Repository, IFeatureRepository
    {
        public FeatureRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<Feature> GetAll()
        {
            return _context.Features;
        }

        public Feature? Create(Feature feature)
        {
            feature = _context.Features.Add(feature).Entity;
            _context.SaveChanges();

            return feature;
        }

        public Feature? Get(int id)
        {
            return GetAll().FirstOrDefault(x => x.Id.Equals(id));
        }

        public Feature? Update(Feature feature)
        {
            _context.Features.Update(feature);
            _context.SaveChanges();

            return feature;
        }

        public void Delete(Feature feature)
        {
            _context.Features.Remove(feature);
            _context.SaveChanges();
        }
    }
}
