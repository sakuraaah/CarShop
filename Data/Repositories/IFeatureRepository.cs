namespace CarShop.Data
{
    public interface IFeatureRepository
    {
        IQueryable<Feature> GetAll();
        IQueryable<Feature> GetByName(string[] features);
        Feature? Create(Feature feature);
        Feature? Get(int id);
        Feature? Update(Feature feature);
        void Delete(Feature feature);
    }
}
