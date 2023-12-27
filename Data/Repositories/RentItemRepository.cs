using CarShop.Dtos;
using CarShop.Models;
using Microsoft.EntityFrameworkCore;

namespace CarShop.Data
{
    public class RentItemRepository : Repository, IRentItemRepository
    {
        public RentItemRepository(ApplicationDbContext context) : base(context) { }

        public IQueryable<RentItem> GetAll()
        {
            return _context.RentItems;
        }

        public RentItemListResponseDto GetList(RentItemQueryDto query, string userRole = "Buyer")
        {
            IQueryable<RentItem> rentItemQuery = GetAll()
                .Include(x => x.User)
                .Include(x => x.Mark)
                .Include(x => x.CarClass)
                .Include(x => x.RentCategory)
                .Include(x => x.Features);

            if (!string.IsNullOrWhiteSpace(query.Username))
            {
                rentItemQuery = rentItemQuery.Where(x => x.User.UserName.Equals(query.Username));
            }
            if (!string.IsNullOrWhiteSpace(query.Category))
            {
                rentItemQuery = rentItemQuery.Where(x => x.Category.Name.Contains(query.Category));
            }
            if (!string.IsNullOrWhiteSpace(query.Mark))
            {
                rentItemQuery = rentItemQuery.Where(x => x.Mark.Name.Contains(query.Mark));
            }
            if (!string.IsNullOrWhiteSpace(query.Model))
            {
                rentItemQuery = rentItemQuery.Where(x => x.Model.Contains(query.Model));
            }
            if (!string.IsNullOrWhiteSpace(query.BodyType))
            {
                rentItemQuery = rentItemQuery.Where(x => x.BodyType.Name.Contains(query.BodyType));
            }
            if (!string.IsNullOrWhiteSpace(query.Color))
            {
                rentItemQuery = rentItemQuery.Where(x => x.Color.Name.Contains(query.Color));
            }
            if (query.Seats != null)
            {
                rentItemQuery = rentItemQuery.Where(x => x.Seats.Equals(query.Seats));
            }
            if (!string.IsNullOrWhiteSpace(query.FeatureList))
            {
                string[] requiredFeatures = query.FeatureList.Split(',');

                //rentItemQuery = rentItemQuery.Where(
                //    x => requiredFeatures.All(
                //        reqFeature => x.Features.Any(feature => feature.Name.Equals(reqFeature))
                //    )
                //);

                foreach (string reqFeature in requiredFeatures)
                {
                    rentItemQuery = rentItemQuery.Where(x => x.Features.Any(f => f.Name.Equals(reqFeature)));
                }
            }
            if (!string.IsNullOrWhiteSpace(query.RentCategory))
            {
                rentItemQuery = rentItemQuery.Where(x => x.RentCategory.Name.Contains(query.RentCategory));
            }
            if (!string.IsNullOrWhiteSpace(query.CarClass))
            {
                rentItemQuery = rentItemQuery.Where(x => x.CarClass.Name.Contains(query.CarClass));
            }
            if (query.PriceFrom != null)
            {
                rentItemQuery = rentItemQuery.Where(x => x.Price >= query.PriceFrom);
            }
            if (query.PriceTo != null)
            {
                rentItemQuery = rentItemQuery.Where(x => x.Price <= query.PriceTo);
            }
            if (query.MileageFrom != null)
            {
                rentItemQuery = rentItemQuery.Where(x => x.Mileage >= query.MileageFrom);
            }
            if (query.MileageTo != null)
            {
                rentItemQuery = rentItemQuery.Where(x => x.Mileage <= query.MileageTo);
            }
            if (query.YearFrom != null)
            {
                rentItemQuery = rentItemQuery.Where(x => x.Year >= query.YearFrom);
            }
            if (query.YearTo != null)
            {
                rentItemQuery = rentItemQuery.Where(x => x.Year <= query.YearTo);
            }

            if (query.StartDate != null)
            {
                rentItemQuery = rentItemQuery.Where(x => x.Created >= query.StartDate);
            }
            if (query.EndDate != null)
            {
                rentItemQuery = rentItemQuery.Where(x => x.Created <= query.EndDate);
            }

            switch (userRole)
            {
                case "Buyer":
                    rentItemQuery = rentItemQuery
                        .Where(x => x.Status.Equals("Submitted"))
                        .Where(x => x.AdminStatus != "Blocked");
                    break;

                case "Seller":
                    if (!string.IsNullOrWhiteSpace(query.Status))
                    {
                        rentItemQuery = rentItemQuery.Where(x => x.Status.Equals(query.Status));
                    }
                    break;

                case "Admin":
                    if (!string.IsNullOrWhiteSpace(query.Status))
                    {
                        rentItemQuery = rentItemQuery.Where(x => x.Status.Equals(query.Status));
                    }
                    rentItemQuery = rentItemQuery.Where(x => x.Status != "Draft");
                    break;

                default:
                    break;
            }

            switch (query.orderDirection)
            {
                case "asc":
                    rentItemQuery = rentItemQuery.OrderBy(x => EF.Property<object>(x, query.orderBy));
                    break;
                case "desc":
                    rentItemQuery = rentItemQuery.OrderByDescending(x => EF.Property<object>(x, query.orderBy));
                    break;
                default:
                    // Default to ascending order if orderDirection is not specified or invalid
                    rentItemQuery = rentItemQuery.OrderBy(x => EF.Property<object>(x, query.orderBy));
                    break;
            }

            var totalCount = rentItemQuery.Count();

            var rentItems = rentItemQuery
                .Select(x => new RentItemListResponseItemDto(x))
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToArray()
                .ToList();


            return new RentItemListResponseDto
            {
                Total = totalCount,
                Page = query.Page,
                PageSize = query.PageSize,
                Results = rentItems
            };
        }

        public bool Exists(int rentSubmissionId, int? id)
        {
            IQueryable<RentItem> rentItemQuery = GetAll()
                .Where(x => x.RentSubmissionId.Equals(rentSubmissionId))
                .Where(x => x.Status != "Cancelled");

            if (id != null)
            {
                rentItemQuery = rentItemQuery.Where(x => x.Id != id);
            }

            return (rentItemQuery.Count() > 0);
        }

        public RentItemResponseDto? Create(RentItem rentItem)
        {
            rentItem = _context.RentItems.Add(rentItem).Entity;
            _context.SaveChanges();

            RentItemResponseDto rentItemResponseDto = new RentItemResponseDto(rentItem);

            return rentItemResponseDto;
        }

        public RentItem? Get(int id, ApplicationUser? user)
        {
            IQueryable<RentItem> rentItemQuery = GetAll()
                .Include(x => x.Category)
                .Include(x => x.Mark)
                .Include(x => x.BodyType)
                .Include(x => x.Color)
                .Include(x => x.RentCategory)
                .Include(x => x.CarClass)
                .Include(x => x.Features)
                .Include(x => x.AvailableStatusTransitions)
                .Where(x => x.Id.Equals(id));

            if (user != null)
            {
                rentItemQuery = rentItemQuery.Where(x => x.User.Equals(user));
            }
            else
            {
                rentItemQuery = rentItemQuery.Where(x => x.Status != "Draft");
            }

            return rentItemQuery.FirstOrDefault();
        }

        public RentItemResponseDto? GetItem(int id, ApplicationUser? user, string userRole = "Buyer")
        {
            IQueryable<RentItem> rentItemQuery = GetAll()
                .Include(x => x.User)
                .Include(x => x.Category)
                .Include(x => x.Mark)
                .Include(x => x.BodyType)
                .Include(x => x.Color)
                .Include(x => x.RentCategory)
                .Include(x => x.CarClass)
                .Include(x => x.Features)
                .Include(x => x.AvailableStatusTransitions)
                .Where(x => x.Id.Equals(id));

            switch (userRole)
            {
                case "Buyer":
                    rentItemQuery = rentItemQuery
                        .Where(x => x.Status.Equals("Submitted"))
                        .Where(x => x.AdminStatus != "Blocked");
                    break;

                case "Seller":
                    rentItemQuery = rentItemQuery.Where(x => x.Status.Equals("Submitted") || x.User.Equals(user));
                    break;

                case "Admin":
                    rentItemQuery = rentItemQuery.Where(x => x.Status != "Draft");
                    break;

                default:
                    break;
            }

            return rentItemQuery
                .Select(x => new RentItemResponseDto(x))
                .FirstOrDefault();
        }

        public RentItemResponseDto? Update(RentItem rentItem)
        {
            _context.RentItems.Update(rentItem);
            _context.SaveChanges();

            RentItemResponseDto buyItemResponseDto = new RentItemResponseDto(rentItem);

            return buyItemResponseDto;
        }

        public void Delete(RentItem rentItem)
        {
            _context.RentItems.Remove(rentItem);
            _context.SaveChanges();
        }
    }
}
