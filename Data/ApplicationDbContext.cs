using CarShop.Models;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CarShop.Data
{
    public class ApplicationDbContext : ApiAuthorizationDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options, IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {

        }
        public DbSet<RentSubmission> RentSubmissions { get; set; }
        public DbSet<RentItem> RentItems { get; set; }
        public DbSet<BuyItem> BuyItems { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Mark> Marks { get; set; }
        public DbSet<BodyType> BodyTypes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<CarClass> CarClasses { get; set; }
        public DbSet<RentCategory> RentCategories { get; set; }
    }
}